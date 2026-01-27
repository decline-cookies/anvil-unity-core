using UnityEngine;
using System.Collections;
using System;

namespace Anvil.Unity.Audio
{
  public class AudioInstance : AbstractAnvilMonoBehaviour
  {
    public const string NAME_PREFIX = "[" + nameof(AudioInstance) + "] - ";

    public event Action<AudioEventArgs> OnComplete;
    public event Action<AudioEventArgs> OnRepeated;

    private AudioManager m_Manager;
    private AudioGroup m_Group;
    private AudioSource m_Source;
    private bool m_IsMuted;
    private float m_Volume;
    private float m_StartTimeSeconds;
    private int m_Loops;
    private int m_TimesPlayed;
    private float m_PauseTime;

    public int TimesPlayed
    {
      get { return m_TimesPlayed; }
    }

    public bool IsPlaying
    {
      get { return m_Source.isPlaying; }
    }

    public bool IsMuted
    {
      get { return m_IsMuted; }
      set
      {
        m_IsMuted = value;
        RefreshIsMuted();
      }
    }

    public float Volume
    {
      get { return m_Volume; }
      set
      {
        m_Volume = value;
        RefreshVolume();
      }
    }

    public Vector3 WorldPosition
    {
      get { return transform.position; }
      set { transform.position = value; }
    }

    public Vector3 LocalPosition
    {
      get { return transform.localPosition; }
      set { transform.localPosition = value; }
    }

    public Transform Parent
    {
      get { return transform.parent; }
      set { transform.parent = value; }
    }

    public AudioManager Manager
    {
      get { return m_Manager; }
      set
      {
        m_Manager = value;
        transform.parent = m_Manager.AudioElementPoolHolder;
      }
    }

    public AudioGroup Group
    {
      get { return m_Group; }
      set
      {
        if (value == m_Group)
        {
          return;
        }

        m_Group?.RemoveElement(this);
        m_Group = value;
        m_Group?.AddInstance(this);
        RefreshIsMuted();
        RefreshVolume();
      }
    }

    public int Loops
    {
      get { return m_Loops; }
      set { m_Loops = value; }
    }

    public float StartTimeSeconds
    {
      get { return m_StartTimeSeconds; }
      set { m_StartTimeSeconds = value; }
    }

    public AudioClip Clip
    {
      get { return m_Source.clip; }
    }

    public AudioSource Source
    {
      get { return m_Source; }
    }

    public override void Awake()
    {
      base.Awake();

      m_Source = gameObject.AddComponent<AudioSource>();
      m_Source.playOnAwake = false;
      m_IsMuted = false;
      m_Volume = 1f;
      UpdateName();

      GameObject.DontDestroyOnLoad(gameObject);
    }

    protected override void DisposeSelf()
    {
      if (m_Manager != null && m_Manager.UpdateHandle != null)
      {
        m_Manager.UpdateHandle.OnUpdate -= OnUpdate;
      }

      Clean();

      m_Manager = null;
      OnComplete = null;
      OnRepeated = null;

      m_Source.Stop();
      m_Source.clip = null;

      base.DisposeSelf();
    }

    public void Init(AudioClip clip)
    {
      Debug.Assert(clip != null, $"Audio must be initialized with a non-null {nameof(AudioClip)}.");
      Debug.Assert(Clip == null, $"Audio has already been initialized with an {nameof(AudioClip)}.");

      m_Source.clip = clip;
      UpdateName();
    }

    public void Clean()
    {
      m_Group?.RemoveElement(this);
      m_Group = null;

      OnComplete = null;
      OnRepeated = null;

      m_Source.clip = null;
      m_Volume = 1f;
      m_IsMuted = false;
      Parent = m_Manager.AudioElementPoolHolder;
      WorldPosition = Vector3.zero;
      UpdateName();
    }

    public void Play()
    {
      if (m_Manager == null)
      {
        return;
      }

      RefreshVolume();
      RefreshIsMuted();
      m_TimesPlayed = 0;
      m_Source.time = 0f;
      m_Source.Play();
      if (m_StartTimeSeconds > 0f)
      {
        m_Source.time = m_StartTimeSeconds * (m_Source.timeSamples / Clip.frequency);
      }
      m_Manager.UpdateHandle.OnUpdate += OnUpdate;
    }

    private void OnUpdate(object _sender, EventArgs _e)
    {
      if (!m_Source.isPlaying)
      {
        return;
      }

      m_TimesPlayed++;
      OnRepeated?.Invoke(this, new AudioEventArgs(this));

      if (m_Loops == -1 || (m_Loops > 0 && m_TimesPlayed < m_Loops))
      {
        m_Source.time = 0f;
        m_Source.Play();
      }
      else
      {
        Stop();
      }
    }

    public void Pause()
    {
      if (m_Manager == null)
      {
        return;
      }

      m_Manager.UpdateHandle.OnUpdate -= OnUpdate;
      m_PauseTime = m_Source.time;
      m_Source.Pause();
    }

    public void Resume()
    {
      if (m_Manager == null)
      {
        return;
      }

      m_Source.time = m_PauseTime;
      m_Source.Play();

      //Remove first to prevent multiple adds
      m_Manager.UpdateHandle.OnUpdate -= OnUpdate;
      m_Manager.UpdateHandle.OnUpdate += OnUpdate;
    }

    public void Stop()
    {
      if (m_Manager == null)
      {
        return;
      }

      m_Source.Stop();

      if (m_Manager.UpdateHandle != null)
      {
        m_Manager.UpdateHandle.OnUpdate -= OnUpdate;
      }

      OnComplete?.Invoke(this, new AudioEventArgs(this));
    }

    public void RefreshIsMuted()
    {
      m_Source.mute = m_IsMuted || m_Group?.IsMuted || m_Manager.IsMasterMuted;
    }

    public void RefreshVolume()
    {
      m_Source.volume = m_Volume * m_Group?.Volume * m_Manager.MasterVolume;
    }

    private void UpdateName()
    {
      gameObject.name = NAME_PREFIX + Clip?.name;
    }
  }
}
