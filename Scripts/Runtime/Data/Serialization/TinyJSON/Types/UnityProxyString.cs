using System;
using TinyJSON;
using UnityEngine;

namespace Anvil.Unity.Data
{
    /// <summary>
    /// Extension of <see cref="ProxyString"/> for Unity specific conversion.
    /// </summary>
    public class UnityProxyString : ProxyString
    {
        public UnityProxyString(string value) : base(value)
        {
        }

        public override object ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == typeof(Color32))
            {
                return ToColor32();
            }

            if (conversionType == typeof(Color))
            {
                return (Color)ToColor32();
            }

            return base.ToType(conversionType, provider);
        }

        private Color32 ToColor32()
        {
            byte r = Convert.ToByte(value.Substring(2, 2), 16);
            byte g = Convert.ToByte(value.Substring(4, 2), 16);
            byte b = Convert.ToByte(value.Substring(6, 2), 16);
            //0xRRGGBB
            byte a = byte.MaxValue;
            //0xRRGGBBAA
            if (value.Length == 10)
            {
                a = Convert.ToByte(value.Substring(8, 2), 16);
            }
            return new Color32(r, g, b, a);
        }
    }
}
