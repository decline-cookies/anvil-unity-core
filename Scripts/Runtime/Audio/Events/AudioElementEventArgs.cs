using UnityEngine;
using System.Collections;
using System;

namespace Anvil.Unity.Audio
{
  public class AudioEventArgs : EventArgs
  {
    public AudioInstance Instance { get; readonly set; }

    public AudioEventArgs(AudioInstance instance)
    {
      Instance = instance;
    }
  }
}

