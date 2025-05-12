
/******************************************************************************************************************
 * 
 * 
 * 标  题： 窗体处理类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2017/06/06
 * 修  改：
 * 参  考： 
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/

using System;

namespace Hikari.Mvvm.Forms
{
    /// <summary>
    /// 窗体处理类
    /// </summary>
    public class FormProcess
    {
        /// <summary>
        /// 禁止程序重复打开
        /// </summary>
        public static void OnStartup()
        {
            // 获取当前活动的进程
            System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
            // 获取当前本地计算机上指定的进程名称的所有进程
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(current.ProcessName);
            foreach (System.Diagnostics.Process process in processes)
            {
                // 忽略当前进程
                if (process.Id != current.Id)
                {
                    if (process.ProcessName.Equals(System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name))
                    {
                        Environment.Exit(Environment.ExitCode);
                        return;
                    }
                }
            }
        }
    }
}