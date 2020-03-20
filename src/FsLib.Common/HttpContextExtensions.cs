using System.Linq;
using Microsoft.AspNetCore.Http;

namespace System
{
    public static class HttpContextExtensions
    {
        public static string GetUserIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            if (ip == null || !IsIp(ip))
            {
                ip = "127.0.0.1";
            }
            return ip;
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static bool IsIp(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}