
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
namespace System.Forms
{
    /// <summary>
    /// 窗体处理类
    /// </summary>
    public class FormProcess
    {
        /// <summary>
        /// 禁止程序重复打开
        /// </summary>
        /// <remarks>http://www.cnblogs.com/SkySoot/archive/2011/11/25/2263878.html</remarks>
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
                    if (process.ProcessName.Equals(System.Reflection.Assembly.GetEntryAssembly().GetName().Name))
                    {
                        HandleRunningInstance(process);
                        Environment.Exit(Environment.ExitCode);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 设置由不同线程产生的窗口的显示状态
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分。</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零。</returns>
        /// <remarks>这个函数向给定窗口的消息队列发送show-window事件。应用程序可以使用这个函数避免在等待一个挂起的应用程序完成处理show-window事件时也被挂起。</remarks>
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        /// <summary>
        /// 将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。系统给创建前台窗口的线程分配的权限稍高于其他线程。
        /// </summary>
        /// <param name="hWnd">将要设置前台的窗口句柄</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零。</returns>
        /// <remarks>前台窗口是z序顶部的窗口，是用户的工作窗口。在一个多任务优先抢占环境中，应让用户控制前台窗口。</remarks>
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private const int WS_SHOWNORMAL = 1;

        /// <summary>
        /// 如果有另一个同名进程启动,则调用之前的实例
        /// </summary>
        /// <param name="instance"></param>
        private static void HandleRunningInstance(System.Diagnostics.Process instance)
        {
            // 确保窗体不是最小化或者最大化
            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL);
            // 将之前启动的进程实例弄到前台窗口
            SetForegroundWindow(instance.MainWindowHandle);
        }
    }
}