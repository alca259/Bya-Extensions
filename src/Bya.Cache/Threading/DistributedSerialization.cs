using System.Text;
using System.Text.Json;

namespace Bya.Cache.Threading;

internal static class DistributedSerialization
{
    public static byte[] ToByteArray(this object obj)
    {
        if (obj == null)
        {
            return null;
        }

        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
    }

    public static T FromByteArray<T>(this byte[] byteArray) where T : class
    {
        if (byteArray == null)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(byteArray);
    }
}