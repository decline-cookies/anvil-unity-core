using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Anvil.Unity.Audio
{
  public class AudioManager : AbstractAnvilMonoBehaviour
  {
    private const int INSTANCE_POOL_MINIMUM_SIZE = 5;
    private const int INSTANCE_POOL_GROWTH_STEP = 10;
    private static AudioInstance s_MockAudioInstance;


    [RuntimeInitializeOnLoad(RuntimeInitializeOnLoad.Subsystem)]
    private static void Init()
    {
      Janitor.SafeDispose(s_MockAudioInstance);

      s_MockAudioInstance = new GameObject("MockAudioElement")
        .AddComponent<AudioInstance>();
    }

    public static AudioManager Create(string name, int pooledInstanceMinimumCount, int pooledInstanceGrowthCount)
    {
      AudioManager manager = new GameObject(name)
        .AddComponent<AudioManager>();

      manager.m_PooledInstanceMinimumCount = pooledInstanceMinimumCount;
      manager.m_PooledInstanceGrowthStep = pooledInstanceGrowthCount;

      return manager;
    }


    [SerializeField]
    private int m_PooledInstanceMinimumCount = INSTANCE_POOL_MINIMUM_SIZE;
    [SerializeField]
    private int m_PooledInstanceGrowthStep = INSTANCE_POOL_GROWTH_STEP;

    private Dictionary<int, AudioGroup> m_Groups;

    private Pool<AudioInstance> m_InstancePool;
    private Dictionary<string, AudioInstance> m_ActiveUniquePlayInstances;
    private List<AudioInstance> m_PendingInstanceReturn;
    private UpdateHandle m_UpdateHandle;

    private Transform m_PoolInstanceContainer;
    private Transform m_ActiveInstanceContainer;

    private float m_Volume = 1f;
    private bool m_IsMuted = false;


    public float Volume
    {
      get { return m_Volume; }
      set
      {
        if (value != m_Volume)
        {
          m_Volume = value;
          foreach (AudioGroup group in m_Groups.Values)
          {
            group.RefreshVolume();
          }
        }
      }
    }

    public bool IsMuted
    {
      get { return m_IsMuted; }
      set
      {
        if (value != m_IsMuted)
        {
          m_IsMuted = value;
          foreach (AudioGroup group in m_Groups.Values)
          {
            group.RefreshIsMuted();
          }
        }
      }
    }

    public override void Awake()
    {
      base.Awake();

      m_Groups = new Dictionary<byte, AudioGroup>();

      m_InstancePool = new Queue<AudioInstance>();
      m_ActiveUniquePlayInstances = new Dictionary<string, AudioInstance>();
      m_PendingInstanceReturn = new List<AudioInstance>();
      m_UpdateHandle = UpdateHandle.Create("Audio Manager");
      m_UpdateHandle.OnUpdate += HandleOnUpdate;

      m_PoolInstanceContainer = new GameObject("[AUDIO INSTANCE POOL]", transform);
      GameObject.DontDestroyOnLoad(m_PoolInstanceContainer);
      m_ActiveInstanceContainer = new GameObject("[ACTIVE AUDIO INSTANCES]", transform);
      GameObject.DontDestroyOnLoad(m_ActiveInstanceContainer);
    }

    protected override void DisposeSelf()
    {
      m_PendingInstanceReturn.DisposeAllAndClear();
      m_ActiveUniquePlayInstances.DisposeAllAndClear();
      m_Groups.DisposeAllAndClear();
      m_InstancePool.Dispose();
      m_UpdateHandle.Dispose();

      base.DisposeSelf();
    }

    private void UpdateHandle_OnUpdate()
    {
      foreach (AudioInstance instance in m_PendingInstanceReturn)
      {
        ReturnElement(instance);
      }
      m_PendingInstanceReturn.Clear();
    }

    private void ReturnInstance(AudioInstance instance)
    {
      //Clean the element
      instance.Clean();
      m_InstancePool.Release(instance);
    }

    private AudioGroup GetOrCreateGroup(byte id)
    {
      if (!m_Groups.TryGet(id, out AudioGroup group))
      {
        group = m_Groups.Add(id, new AudioGroup(id));
      }

      return group;
    }

    //Plays a sound only if none of the other sounds in the candidates array are currently playing. ONLY sounds played through PlayUnique are affected.
    //For example, if you play a Sound called BIG_HIT via Play() and then you play the same sound BIG_HIT via PlayUnique(), BOTH sounds will play. 
    //If you play a Sound called BIG_HIT via PlayUnique() and then you play the same sound BIG_HIT via PlayUnique(), only one will play.
    //The optional _priorityOverrides array specifies other sounds in order of priority with the last element being the most important. 
    //It will iterate through the array and stop the sound in the array if it is playing until after it finds your sound specified by the _clipKey. 
    //In an array of 5 elements with _clipKey being the middle, the following rules will apply
    // SOUND0 - (SOUND0 will be stopped if already playing, _clipKey has priority and may still play)
    // SOUND1 - (SOUND1 will be stopped if already playing, _clipKey has priority and may still play)
    // _clipKey - (Will not play if _clipKey is already playing)
    // SOUND3 - (_clipKey will not play if SOUND3 is already playing)
    // SOUND4 - (_clipKey will not play if SOUND4 is already playing)
    public bool TryPlayUnique(out AudioInstance instance, AudioClip clip, int groupID = -1, int repeatCount = 0, float startTimeSeconds = 0.0f, Transform parent = null, IEnumerable<AudioClip> fallbackClips = null)
    {
      if (IsDisposed)
      {
        instance = null;
        return false;
      }

      // Find a unique clip to play and abort if there aren't any.
      if (m_ActiveUniquePlayInstances.ContainsKey(clip) && !TrySelectUniqueClip(clip, out clip))
      {
        instance = null;
        return false;
      }

      AudioInstance instance = Play(clip, groupID, repeatCount, startTimeSeconds, parent);
      m_ActiveUniquePlayInstances.Add(clip, instance);
      instance.OnComplete += UniqueAudioInstance_OnAudioComplete;

      return true;
    }

    private bool TrySelectUniqueClip(IEnumerable<AudioClip> clips, out AudioClip uniqueClip)
    {
      foreach (AudioClip clip in clips)
      {
        if (!m_ActiveUniquePlayInstances.ContainsKey(uniqueClip))
        {
          uniqueClip = clip;
          return true;
        }
      }

      uniqueClip = null;
      return false;
    }

    private void UniqueAudioInstance_OnAudioComplete(AudioEventArgs e)
    {
      AudioInstance element = e.Instance;
      element.OnComplete -= HandleOnUniqueAudioComplete;
      m_ActiveUniquePlayInstances.Remove(element.ClipKey);

      //TODO: Return to pool?!
    }

    public AudioInstance Play(AudioClip clip, int groupID = -1, int repeatCount = 0, float startTimeSeconds = 0.0f, Transform parent = null)
    {
      if (IsDisposed)
      {
        return s_MockAudioInstance;
      }

      AudioGroup group = GetOrCreateGroup(groupID);
      AudioInstance instance = m_InstancePool.Acquire();
      instance.Init(clip);
      instance.Parent = (parent == null) ? m_ActiveInstanceContainer : parent;
      instance.Group = group;
      instance.Loops = repeatCount;
      instance.StartTimeSeconds = startTimeSeconds;
      instance.OnComplete += HandleOnAudioComplete;

      instance.Play();

      return instance;
    }

    private void HandleOnAudioComplete(AudioEventArgs e)
    {
      AudioInstance element = e.Instance;
      element.OnComplete -= HandleOnAudioComplete;

      if (m_PendingInstanceReturn == null || IsDisposed)
      {
        return;
      }

      m_PendingInstanceReturn.Add(element);
    }

    public void StopAll()
    {
      if (IsDisposed)
      {
        return;
      }

      foreach (AudioGroup group in m_Groups.Values)
      {
        group.StopAll();
      }
    }

    public void PauseAll()
    {
      if (IsDisposed)
      {
        return;
      }

      foreach (AudioGroup group in m_Groups.Values)
      {
        group.PauseAll();
      }
    }

    public void ResumeAll()
    {
      if (IsDisposed)
      {
        return;
      }

      foreach (AudioGroup group in m_Groups.Values)
      {
        group.ResumeAll();
      }
    }

  }
}

