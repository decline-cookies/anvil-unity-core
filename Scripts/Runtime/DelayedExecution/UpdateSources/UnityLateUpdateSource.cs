namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// A specific <see cref="AbstractUnityUpdateSource"/> that dispatches an OnUpdate event to an
    /// <see cref="UpdateHandle"/> when LateUpdate is called by Unity.
    /// </summary>
    public class UnityLateUpdateSource : AbstractUnityUpdateSource<UnityLateUpdateSourceMonoBehaviour>
    {
    }
}