namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// A specific <see cref="AbstractUnityUpdateSource"/> that dispatches an OnUpdate event to an
    /// <see cref="UpdateHandle"/> when Update is called by Unity.
    /// </summary>
    public class UnityUpdateSource : AbstractUnityUpdateSource<UnityUpdateSourceMonoBehaviour>
    {
    }
}