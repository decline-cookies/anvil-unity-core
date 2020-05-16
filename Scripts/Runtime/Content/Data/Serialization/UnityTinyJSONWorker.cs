using System;
using System.Collections.Generic;
using Anvil.CSharp.Data;
using TinyJSON;
using UnityEngine;

#if ENABLE_IL2CPP
using UnityEngine.Scripting;
#endif

namespace Anvil.Unity.Data
{

#if ENABLE_IL2CPP
	[Preserve]
#endif
    public class UnityTinyJSONWorker : TinyJSONWorker
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            JSON.OverrideInstance(new UnityTinyJSONWorker());
        }

        public override int Priority { get; } = 1;

#if ENABLE_IL2CPP
		[Preserve]
#endif
        protected override T DecodeType<T>(Variant data)
        {
            return base.DecodeType<T>(data);
        }

#if ENABLE_IL2CPP
		[Preserve]
#endif
        protected override List<T> DecodeList<T>(Variant data)
        {
            return base.DecodeList<T>(data);
        }

#if ENABLE_IL2CPP
		[Preserve]
#endif
        // ReSharper disable once UnusedMethodReturnValue.Local
        protected override Dictionary<TKey, TValue> DecodeDictionary<TKey, TValue>(Variant data)
        {
            return base.DecodeDictionary<TKey, TValue>(data);
        }

#if ENABLE_IL2CPP
		[Preserve]
#endif
        protected override T[] DecodeArray<T>( Variant data )
        {
            return base.DecodeArray<T>(data);
        }

#if ENABLE_IL2CPP
		[Preserve]
#endif
        protected override void DecodeMultiRankArray<T>(ProxyArray arrayData, Array array, int arrayRank, int[] indices)
        {
            base.DecodeMultiRankArray<T>(arrayData, array, arrayRank, indices);
        }

#if ENABLE_IL2CPP
		[Preserve]
#endif
        public override void SupportTypeForAOT<T>()
        {
            base.SupportTypeForAOT<T>();
        }

#if ENABLE_IL2CPP
		[Preserve]
#endif
        public override void SupportValueTypesForAOT()
        {
            base.SupportValueTypesForAOT();
        }
    }
}

