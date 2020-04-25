namespace Anvil.Unity.DelayedExecution
{
    /// <summary>
    /// A specific <see cref="AbstractUnityUpdateSource"/> that dispatches
    /// an <see cref="AbstractUnityUpdateSource{T}.OnUpdate"/> event to an
    /// <see cref="UpdateHandle"/> when <see cref="MonoBehaviour.FixedUpdate"/> is called by Unity.
    /// <see cref="UnityFixedUpdateSourceMonoBehaviour"/>
    /// </summary>
    public class UnityFixedUpdateSource : AbstractUnityUpdateSource<UnityFixedUpdateSourceMonoBehaviour>
    {
    }
}