namespace Anvil.Unity.Audio
{
  public interface IAudioGroupControl
  {
    public byte ID { get; }
    public float Volume { get; set; }
    public bool IsMuted { get; set; }

    public void PauseAll();
    public void ResumeAll();
    public void StopAll();
  }
}