using System;
using System.Collections.Generic;
using TinyJSON;
using UnityEngine;

namespace Anvil.Unity.Data
{
    /// <summary>
    /// Extension of <see cref="Encoder{TProxyArray,TProxyBoolean,TProxyNumber,TProxyObject,TProxyString}"/> for
    /// Unity specific encoding.
    /// </summary>
    public class UnityEncoder : Encoder<ProxyArray, ProxyBoolean, ProxyNumber, ProxyObject, UnityProxyString>
    {
        protected override void EncodeValue(object value, bool forceTypeHint)
        {
            if (value is Vector2Int vec2)
            {
                EncodeObject( (Vector2)vec2, forceTypeHint );
                return;
            }

            if (value is Vector3Int vec3)
            {
                EncodeObject( (Vector3)vec3, forceTypeHint );
                return;
            }

            if (value is Color || value is Color32)
            {
                Color32 color32;

                if (value is Color color)
                {
                    color32 = color;
                }
                else
                {
                    color32 = (Color32)value;
                }

                List<byte> data = new List<byte>{ color32.r, color32.g, color32.b };
                if (color32.a < byte.MaxValue)
                {
                    data.Add( color32.a );
                }

                EncodeString( "0x" + BitConverter.ToString( data.ToArray() ).Replace( "-", string.Empty ) );
                return;
            }

            base.EncodeValue(value, forceTypeHint);
        }
    }
}
