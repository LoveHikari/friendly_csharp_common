using System.Net;
using System.Net.Sockets;

namespace Hikari.Common.Net;
/// <summary>
/// <see cref="IPAddress"/> 扩展类
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class IPAddressExtensions
{
    /// <summary>
    /// 判断IP是否是私有地址
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static bool IsPrivateIP(this IPAddress ip)
    {
        if (IPAddress.IsLoopback(ip)) return true;
        ip = ip.IsIPv4MappedToIPv6 ? ip.MapToIPv4() : ip;
        byte[] bytes = ip.GetAddressBytes();
        return ip.AddressFamily switch
        {
            AddressFamily.InterNetwork when bytes[0] == 10 => true,
            AddressFamily.InterNetwork when bytes[0] == 100 && bytes[1] >= 64 && bytes[1] <= 127 => true,
            AddressFamily.InterNetwork when bytes[0] == 169 && bytes[1] == 254 => true,
            AddressFamily.InterNetwork when bytes[0] == 172 && bytes[1] == 16 => true,
            AddressFamily.InterNetwork when bytes[0] == 192 && bytes[1] == 88 && bytes[2] == 99 => true,
            AddressFamily.InterNetwork when bytes[0] == 192 && bytes[1] == 168 => true,
            AddressFamily.InterNetwork when bytes[0] == 198 && bytes[1] == 18 => true,
            AddressFamily.InterNetwork when bytes[0] == 198 && bytes[1] == 51 && bytes[2] == 100 => true,
            AddressFamily.InterNetwork when bytes[0] == 203 && bytes[1] == 0 && bytes[2] == 113 => true,
            AddressFamily.InterNetwork when bytes[0] >= 233 => true,
            AddressFamily.InterNetworkV6 when ip.IsIPv6Teredo || ip.IsIPv6LinkLocal || ip.IsIPv6Multicast || ip.IsIPv6SiteLocal || bytes[0] == 0 || bytes[0] >= 252 => true,
            _ => false
        };
    }
    /// <summary>
    /// IPv4地址转数字
    /// </summary>
    /// <param name="ip">IPv4地址</param>
    /// <returns>ip数字</returns>
    public static long IPv4ToInt(this IPAddress ip)
    {
        ip = ip.IsIPv4MappedToIPv6 ? ip.MapToIPv4() : ip;
        var ipv4Str = ip.ToString();
        char[] separator = { '.' };
        string[] items = ipv4Str.Split(separator);
        return long.Parse(items[0]) << 24
               | long.Parse(items[1]) << 16
               | long.Parse(items[2]) << 8
               | long.Parse(items[3]);
    }
}