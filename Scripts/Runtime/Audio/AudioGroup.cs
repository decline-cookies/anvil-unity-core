using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Anvil.Unity.Audio
{
  public class AudioGroup : AbstractDisposable, IAudioGroupControl
  {
    private HashSet<AudioInstance> m_AudioInstances;
    private float m_Volume;
    private bool m_IsMuted;

    public float Volume
    {
      get => m_Volume;
      set
      {
        if (value != m_Volume)
        {
          m_Volume = value;
          UpdateVolume();
        }
      }
    }

    public bool IsMuted
    {
      get => m_IsMuted;
      set
      {
        if (value != m_IsMuted)
        {
          m_IsMuted = value;
          UpdateIsMuted();
        }
      }
    }

    public byte ID { get; readonly set; }


    public AudioGroup(byte id)
    {
      ID = id;
      m_AudioInstances = new List<AudioInstance>();
      m_Volume = 1.0f;
      m_IsMuted = false;
    }

    public void AddInstance(AudioInstance instance)
    {
      m_AudioInstances.Add(instance);
    }

    public void RemoveInstance(AudioInstance instance)
    {
      m_AudioInstances.Remove(instance);
    }

    public void StopAll()
    {
      foreach (AudioInstance instance in m_AudioInstances)
      {
        instance.Stop();
      }
    }

    public void PauseAll()
    {
      foreach (AudioInstance instance in m_AudioInstances)
      {
        instance.Pause();
      }
    }

    public void ResumeAll()
    {
      foreach (AudioInstance instance in m_AudioInstances)
      {
        instance.Resume();
      }
    }

    private void UpdateVolume()
    {
      foreach (AudioInstance instance in m_AudioInstances)
      {
        instance.RefreshVolume();
      }
    }

    private void UpdateIsMuted()
    {
      foreach (AudioInstance instance in m_AudioInstances)
      {
        instance.RefreshIsMuted();
      }
    }
  }
}

