using System.Data;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace Hikari.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemHelper
    {
        /// <summary>
        /// 获取经纬度
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <remarks>http://www.cnblogs.com/huangfr/archive/2012/03/27/2420464.html</remarks>
        public static void GetLocation(string address, out string lng, out string lat)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("http://api.map.baidu.com/geocoder?address=" + address);
            DataTable? dt = ConvertHelper.XmlToDataSet(doc.InnerXml).Tables["location"];
            if (dt is { Rows.Count: > 0 })
            {
                lng = dt.Rows[0]["lng"].ToString() ?? "";
                lat = dt.Rows[0]["lat"].ToString() ?? "";
            }
            else
            {
                lng = "";
                lat = "";
            }
        }
        /// <summary>
        /// 检测端口号
        /// </summary>
        /// <param name="tempPort">端口号</param>
        /// <remarks>http://blog.csdn.net/jayzai/article/details/8182418</remarks>
        /// <returns></returns>
        public static bool CheckPort(string tempPort)
        {
            Process p = new Process
            {
                StartInfo = new ProcessStartInfo("netstat", "-an")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true
                }
            };
            p.Start();
            string result = p.StandardOutput.ReadToEnd().ToLower();//最后都转换成小写字母
            System.Net.IPAddress[] addressList = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList;
            List<string> ipList = new List<string> { "127.0.0.1", "0.0.0.0" };
            foreach (System.Net.IPAddress t in addressList)
            {
                ipList.Add(t.ToString());
            }
            bool use = false;
            foreach (string t in ipList)
            {
                if (result.IndexOf("tcp    " + t + ":" + tempPort, StringComparison.CurrentCultureIgnoreCase) > -1 || result.IndexOf("udp    " + t + ":" + tempPort, StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    use = true;
                    break;
                }
            }
            p.Close();
            return use;
        }

        /// <summary>
        /// 设置环境变量
        /// </summary>
        /// <param name="variable">环境变量名</param>
        /// <param name="value">值</param>
        /// <remarks>http://blog.chinaunix.net/uid-25498312-id-4085179.html</remarks>
        public static void SetVariable(string variable, string value)
        {
            string? pathlist = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Machine);
            if (pathlist != null)
            {
                pathlist = pathlist.TrimEnd(';');
                string[] list = pathlist.Split(';');
                bool isPathExist = false;

                foreach (string item in list)
                {
                    if (item == value)
                        isPathExist = true;
                }
                if (!isPathExist)
                {
                    Environment.SetEnvironmentVariable(variable, pathlist + ";" + value,
                        EnvironmentVariableTarget.Machine);
                }
            }
            else
            {
                Environment.SetEnvironmentVariable(variable, value, EnvironmentVariableTarget.Machine);
            }

        }

        /// <summary>
        /// 执行cmd命令
        /// </summary>
        /// <param name="cmd">cmd命令</param>
        /// <returns>执行结果</returns>
        public static async Task<string> ExecuteCommandAsync(string cmd)
        {
            var fileName = cmd.SplitLeft(" ");
            var arguments = cmd.TrimStart(fileName).TrimStart(' ');
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false, //是否使用操作系统shell启动
                RedirectStandardInput = true, //接受来自调用程序的输入信息
                RedirectStandardOutput = true, //由调用程序获取输出信息
                RedirectStandardError = true, //重定向标准错误输出
                CreateNoWindow = true //不显示程序窗口
            };

            using Process? process = Process.Start(psi);
            if (process != null)
            {
                string output = await process.StandardOutput.ReadToEndAsync();
                if (string.IsNullOrWhiteSpace(output))
                {
                    output = await process.StandardError.ReadToEndAsync();
                }
                await process.WaitForExitAsync();
                return output;
            }
            return "";

        }
        /// <summary>
        /// 执行cmd命令
        /// </summary>
        /// <param name="commands">cmd命令</param>
        /// <returns>执行结果</returns>
        public static async Task<string> ExecuteCommandAsync(params string[] commands)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                RedirectStandardError = true, //重定向标准错误输出
                CreateNoWindow = true
            };
            var outputBuilder = new StringBuilder();

            using Process process = new Process();
            process.StartInfo = psi;

            // 异步读取输出和错误流
            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    outputBuilder.AppendLine(e.Data);
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    outputBuilder.AppendLine(e.Data);
            };

            process.Start();
            // 开始异步读取
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            // 写入多条命令
            await using StreamWriter sw = process.StandardInput;
            await sw.WriteLineAsync("echo off");  // 关闭命令回显
            foreach (var cmd in commands)
            {
                await sw.WriteLineAsync(cmd);
            }
            // 必须显式退出命令提示符
            await sw.WriteLineAsync("exit");

            await process.WaitForExitAsync();
            
            var output = outputBuilder.ToString();

            return output.Trim();
        }

    }
}