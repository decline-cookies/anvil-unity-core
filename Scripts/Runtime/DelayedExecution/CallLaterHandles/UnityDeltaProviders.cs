using Anvil.CSharp.DelayedExecution;
using UnityEngine;

namespace Anvil.Unity.DelayedExecution
{
    public static class UnityDeltaProviders
    {
        public static readonly DeltaProvider Frames = () => 1;
        public static readonly DeltaProvider DeltaTime = () => Time.deltaTime;
        public static readonly DeltaProvider UnscaledDeltaTime = () => Time.unscaledDeltaTime;
        public static readonly DeltaProvider FixedDeltaTime = () => Time.fixedDeltaTime;
        public static readonly DeltaProvider UnscaledFixedDeltaTime = () => Time.fixedUnscaledDeltaTime;
    }
}
