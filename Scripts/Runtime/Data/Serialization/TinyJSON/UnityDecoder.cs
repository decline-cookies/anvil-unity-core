using TinyJSON;

namespace Anvil.Unity.Data
{
    /// <summary>
    /// Extension of <see cref="Decoder{TProxyArray,TProxyBoolean,TProxyNumber,TProxyObject,TProxyString}"/> for
    /// Unity specific decoding.
    /// </summary>
    public class UnityDecoder : Decoder<ProxyArray, ProxyBoolean, ProxyNumber, ProxyObject, UnityProxyString> { }
}
