using System.Threading.Tasks;

using System.Collections.Generic;

namespace DotnetBleServer.Gatt
{
    public interface ICharacteristicSource
    {
        Task WriteValueAsync(byte[] value, IDictionary<string, object> options);
        Task<byte[]> ReadValueAsync(IDictionary<string, object> options);
    }
}