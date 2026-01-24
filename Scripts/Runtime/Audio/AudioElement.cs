using UnityEngine;
using System.Collections;
using System;

namespace Anvil.Unity.Audio
{
  public class AudioInstance : AbstractAnvilMonoBehaviour
  {
    public const string DEFAULT_NAME = "[POOLED AUDIO ELEMENT]";

    public event Action<AudioEventArgs> OnComplete;
    public event Action<AudioEventArgs> OnRepeated;

    private AudioManager m_Manager;
    private AudioGroup m_Group;
    private AudioSource m_Source;
    private bool m_IsMuted;
    private float m_Volume;
    private AudioClip m_Clip;
    private float m_StartTimeSeconds;
    private int m_Loops;
    private int m_TimesPlayed;
    private string m_ClipKey;
    private float m_PauseTime;

    public string ClipKey
    {
      get { return m_ClipKey; }
      set { m_ClipKey = value; }
    }

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
        if (value != m_Group)
        {
          if (m_Group != null)
          {
            m_Group.RemoveElement(this);
          }
          m_Group = value;
          m_Group.AddInstance(this);
        }
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
      get { return m_Clip; }
      set { m_Clip = value; }
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
      m_Volume = 1.0f;
      gameObject.name = DEFAULT_NAME;
      GameObject.DontDestroyOnLoad(gameObject);
    }

    protected override void DisposeSelf()
    {
      if (m_Manager != null && m_Manager.UpdateHandle != null)
      {
        m_Manager.UpdateHandle.OnUpdate -= OnUpdate;
      }
      m_Manager = null;
      m_Group = null;
      m_Clip = null;
      OnComplete = null;
      OnRepeated = null;
      KApplication.OnAppPaused -= HandleOnAppPaused;
      KApplication.OnAppResumed -= HandleOnAppResumed;
      if (m_Source != null)
      {
        m_Source.Stop();
        m_Source.clip = null;
        m_Source = null;
      }

      base.DisposeSelf();
    }


    public void Play()
    {
      if (m_Manager == null || m_Source == null)
      {
        return;
      }
      gameObject.name = m_Clip.name;
      m_Source.clip = m_Clip;
      RefreshVolume();
      RefreshIsMuted();
      m_TimesPlayed = 0;
      m_Source.time = 0.0f;
      m_Source.Play();
      if (m_StartTimeSeconds > 0.0f)
      {
        m_Source.time = m_StartTimeSeconds * (m_Source.timeSamples / m_Clip.frequency);
      }
      m_Manager.UpdateHandle.OnUpdate += OnUpdate;
      KApplication.OnAppPaused += HandleOnAppPaused;
    }

    private void HandleOnAppPaused(object _sender, EventArgs _e)
    {
      KApplication.OnAppPaused -= HandleOnAppPaused;
      KApplication.OnAppResumed += HandleOnAppResumed;
      Pause();
    }

    private void HandleOnAppResumed(object _sender, EventArgs _e)
    {
      KApplication.OnAppResumed -= HandleOnAppResumed;
      KApplication.OnAppPaused += HandleOnAppPaused;
      Resume();
    }

    private void OnUpdate(object _sender, EventArgs _e)
    {
      if (m_Source.isPlaying == false)
      {
        m_TimesPlayed++;
        OnRepeated.Dispatch(this, new AudioEventArgs(this));
        if (m_Loops == -1 || (m_Loops > 0 && m_TimesPlayed < m_Loops))
        {
          m_Source.time = 0.0f;
          m_Source.Play();
        }
        else
        {
          Stop();
        }
      }
    }

    public void Pause()
    {
      if (m_Manager == null || m_Source == null)
      {
        return;
      }
      m_Manager.UpdateHandle.OnUpdate -= OnUpdate;
      m_PauseTime = m_Source.time;
      m_Source.Pause();
    }

    public void Resume()
    {
      if (m_Manager == null || m_Source == null)
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
      if (m_Manager == null || m_Source == null)
      {
        return;
      }
      if (m_Source != null)
      {
        m_Source.Stop();
      }

      if (m_Manager != null && m_Manager.UpdateHandle != null)
      {
        m_Manager.UpdateHandle.OnUpdate -= OnUpdate;
      }
      KApplication.OnAppPaused -= HandleOnAppPaused;
      KApplication.OnAppResumed -= HandleOnAppResumed;
      OnComplete.Dispatch(this, new AudioEventArgs(this));
    }

    public void Clean()
    {
      KApplication.OnAppPaused -= HandleOnAppPaused;
      KApplication.OnAppResumed -= HandleOnAppResumed;
      m_Group.RemoveElement(this);
      m_Group = null;
      OnComplete = null;
      OnRepeated = null;
      m_Source.clip = null;
      m_Clip = null;
      m_Volume = 1.0f;
      m_IsMuted = false;
      Parent = m_Manager.AudioElementPoolHolder;
      gameObject.name = DEFAULT_NAME;
      WorldPosition = Vector3.zero;
    }

    public void RefreshIsMuted()
    {
      m_Source.mute = m_IsMuted || m_Group.IsMuted || m_Manager.IsMasterMuted;
    }

    public void RefreshVolume()
    {
      m_Source.volume = m_Volume * m_Group.Volume * m_Manager.MasterVolume;
    }

  }
}
