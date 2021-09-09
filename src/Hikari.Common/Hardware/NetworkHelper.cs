using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace Hikari.Common.Hardware
{
    /// <summary>
    /// 网卡信息
    /// <remarks>https://github.com/lindexi/UWP/blob/master/uwp/src/MacAddress/MacAddress/MacAddress.cs , https://blog.csdn.net/diedouju3498/article/details/101894078 </remarks>
    /// </summary>
    public class NetworkHelper
    {
        /// <summary>
        /// 获取当前使用的IP
        /// </summary>
        /// <returns></returns>
        public static string GetLocalUsedIPv4()
        {
            var ips = GetLocalIPs();
            var address = ips.FirstOrDefault(i => i.IsDnsEligible == true && i.SuffixOrigin == SuffixOrigin.OriginDhcp);
            return address?.Address.ToString() ?? "127.0.0.1";
        }

        /// <summary>  
        /// 获取本机所有的ip地址
        /// </summary>  
        /// <returns></returns>  
        private static List<UnicastIPAddressInformation> GetLocalIPs()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces().OrderByDescending(c => c.Speed).Where(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up); // 所有网卡信息
            return interfaces.SelectMany(n => n.GetIPProperties().UnicastAddresses).ToList();
        }

        /// <summary>
        /// 获取本地Mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            var adapters = nics.Where(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up); // 所有网卡信息
            adapters = adapters.Where(a => a.GetIPProperties().IsDynamicDnsEnabled);  // DNS 网卡

            var macAddress = adapters.Select(adapter => adapter.GetPhysicalAddress())
                .Select(address => string.Join("-", address.GetAddressBytes().Select(b => $"{b:X2}"))).ToList();

            return macAddress.FirstOrDefault();
        }
    }
}