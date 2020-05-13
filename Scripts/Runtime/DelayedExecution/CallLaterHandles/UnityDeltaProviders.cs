using Anvil.CSharp.DelayedExecution;
using UnityEngine;

namespace Anvil.Unity.DelayedExecution
{
    public static class UnityDeltaProviders
    {
        public static DeltaFramesProvider Frames = () => 1;
        public static DeltaTimeProvider DeltaTime = () => Time.deltaTime;
        public static DeltaTimeProvider UnscaledDeltaTime = () => Time.unscaledDeltaTime;
        public static DeltaTimeProvider FixedDeltaTime = () => Time.fixedDeltaTime;
        public static DeltaTimeProvider UnscaledFixedDeltaTime = () => Time.fixedUnscaledDeltaTime;
    }
}
