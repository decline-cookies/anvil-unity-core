using System;

namespace Anvil.Unity.Audio
{
  public readonly struct AudioEventArgs : EventArgs
  {
    public readonly AudioInstance Instance { get; readonly set; }

    public AudioEventArgs(AudioInstance instance)
    {
      Instance = instance;
    }
  }
}

