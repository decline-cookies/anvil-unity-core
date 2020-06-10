using System;
using Anvil.CSharp.Data;
using TinyJSON;
using UnityEngine;

namespace Anvil.Unity.Data
{
    public class UnityTinyJSONParser : TinyJSONParser<UnityEncoder, UnityDecoder>
    {
        [RuntimeInitializeOnLoadMethod]
        public static void Register()
        {
            JSON.OverrideParser(new UnityTinyJSONParser());
        }

        public override int Priority { get; } = 1;

        protected override bool DTCheckConvert<T>(Variant data, Type type, out T decodedData)
        {
            if (type == typeof(Color)
                || type == typeof(Color32))
            {
                decodedData = (T) Convert.ChangeType( data, type );
                return true;
            }

            return base.DTCheckConvert(data, type, out decodedData);
        }

        protected override bool DTCheckType<T>(Variant data, Type type, out T decodedData)
        {
            return base.DTCheckType(data, type, out decodedData)
                || DTCheckVector2Int(data, type, out decodedData)
                || DTCheckVector3Int(data, type, out decodedData);
        }

        private bool DTCheckVector2Int<T>(Variant data, Type type, out T decodedType)
        {
            if (typeof(Vector2Int).IsAssignableFrom( type ))
            {
                Vector2 value = DecodeType<Vector2>( data );
                decodedType = (T) (object) new Vector2Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
                return true;
            }

            decodedType = default;
            return false;
        }

        private bool DTCheckVector3Int<T>(Variant data, Type type, out T decodedType)
        {
            if (typeof(Vector3Int).IsAssignableFrom( type ))
            {
                Vector3 value = DecodeType<Vector3>( data );
                decodedType = (T) (object) new Vector3Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y), Mathf.RoundToInt(value.z));
                return true;
            }

            decodedType = default;
            return false;
        }
    }
}

