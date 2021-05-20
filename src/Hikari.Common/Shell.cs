using System;
using System.Runtime.InteropServices;

/******************************************************************************************************************
* 
* 
* 标  题：控制台交互类(版本：Version1.0.0)
* 作　者：YuXiaoWei
* 日　期：2016/05/31
* 修　改：
* 参　考：
* 说　明：暂无...
* 备　注：暂无...
* 调用示列：调用AllocConsole方法显示控制台，调用FreeConsole方法关闭控制台
* 
* ***************************************************************************************************************/
namespace Hikari.Common
{
    /// <summary>
    /// 与控制台交互
    /// </summary>
    public class Shell
    {
        /// <summary>
        /// 显示控制台
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]  
        public static extern Boolean AllocConsole();
        /// <summary>
        /// 关闭控制台
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();

        #region 换行输出，带时间
        /// <summary>
        /// 换行输出，带时间
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <example>例：Shell.WriteLine("{0}：{1}", "警告", "这是一条警告信息。");</example>
        public static void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }
        /// <summary>
        /// 换行输出，带时间输出信息
        /// </summary>
        /// <param name="output"></param>
        /// <example>例：Shell.WriteLine("注意：2秒后关闭...");</example>
        public static void WriteLine(string output)
        {
            Console.ForegroundColor = GetConsoleColor(output);
            Console.WriteLine(@"[{0}] {1}", DateTimeOffset.Now, output);
        }
        #endregion

        #region 换行输出，不带时间
        /// <summary>
        /// 换行输出，不带时间
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <example>例：Shell.WriteLine("{0}：{1}", "警告", "这是一条警告信息。");</example>
        public static void WriteLine2(string format, params object[] args)
        {
            WriteLine2(string.Format(format, args));
        }
        /// <summary>
        /// 换行输出，不带时间
        /// </summary>
        /// <param name="output"></param>
        /// <example>例：Shell.WriteLine("注意：2秒后关闭...");</example>
        public static void WriteLine2(string output)
        {
            Console.ForegroundColor = GetConsoleColor(output);
            Console.WriteLine(@"{0}", output);
        }
        #endregion
        #region 不换行输出，不带时间
        /// <summary>
        /// 不换行输出，不带时间
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <example>例：Shell.WriteLine("{0}：{1}", "警告", "这是一条警告信息。");</example>
        public static void Write(string format, params object[] args)
        {
            WriteLine2(string.Format(format, args));
        }
        /// <summary>
        /// 不换行输出，不带时间
        /// </summary>
        /// <param name="output"></param>
        /// <example>例：Shell.WriteLine("注意：2秒后关闭...");</example>
        public static void Write(string output)
        {
            Console.ForegroundColor = GetConsoleColor(output);
            Console.WriteLine(@"{0}", output);
        }
        #endregion

        #region 换行输出
        /// <summary>
        /// 换行输出警告信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <example>例：Shell.WriteLine("{0}：{1}", "警告", "这是一条警告信息。");</example>
        public static void WriteLineWarning(string format, params object[] args)
        {
            WriteLineWarning(string.Format(format, args));
        }
        /// <summary>
        /// 换行输出错误信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <example>例：Shell.WriteLine("{0}：{1}", "警告", "这是一条警告信息。");</example>
        public static void WriteLineError(string format, params object[] args)
        {
            WriteLineError(string.Format(format, args));
        }
        /// <summary>
        /// 换行输出注意信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <example>例：Shell.WriteLine("{0}：{1}", "警告", "这是一条警告信息。");</example>
        public static void WriteLineAttention(string format, params object[] args)
        {
            WriteLineAttention(string.Format(format, args));
        }
        /// <summary>
        /// 换行输出警告信息
        /// </summary>
        /// <param name="output"></param>
        /// <example>例：Shell.Write("注意：2秒后关闭...");</example>
        public static void WriteLineWarning(string output)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"[{0}] {1}", DateTimeOffset.Now, output);
        }
        /// <summary>
        /// 换行输出错误
        /// </summary>
        /// <param name="output"></param>
        /// <example>例：Shell.Write("注意：2秒后关闭...");</example>
        public static void WriteLineError(string output)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"[{0}] {1}", DateTimeOffset.Now, output);
        }
        /// <summary>
        /// 换行输出注意信息
        /// </summary>
        /// <param name="output"></param>
        /// <example>例：Shell.Write("注意：2秒后关闭...");</example>
        public static void WriteLineAttention(string output)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"[{0}] {1}", DateTimeOffset.Now, output);
        }
        #endregion
        #region 不换行输出，不带时间
        /// <summary>
        /// 不换行输出，不带时间（警告）
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <example>例：Shell.WriteLine("{0}：{1}", "警告", "这是一条警告信息。");</example>
        public static void WriteWarning(string format, params object[] args)
        {
            WriteWarning(string.Format(format, args));
        }
        /// <summary>
        /// 不换行输出，不带时间（错误）
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <example>例：Shell.WriteLine("{0}：{1}", "警告", "这是一条警告信息。");</example>
        public static void WriteError(string format, params object[] args)
        {
            WriteError(string.Format(format, args));
        }
        /// <summary>
        /// 不换行输出，不带时间（注意）
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <example>例：Shell.WriteLine("{0}：{1}", "警告", "这是一条警告信息。");</example>
        public static void WriteAttention(string format, params object[] args)
        {
            WriteAttention(string.Format(format, args));
        }
        /// <summary>
        /// 不换行输出，不带时间（警告）
        /// </summary>
        /// <param name="output"></param>
        /// <example>例：Shell.Write("注意：2秒后关闭...");</example>
        public static void WriteWarning(string output)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(@"{0}", output);
        }
        /// <summary>
        /// 不换行输出，不带时间（错误）
        /// </summary>
        /// <param name="output"></param>
        /// <example>例：Shell.Write("注意：2秒后关闭...");</example>
        public static void WriteError(string output)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(@"{0}",output);
        }
        /// <summary>
        /// 不换行输出，不带时间（注意）
        /// </summary>
        /// <param name="output"></param>
        /// <example>例：Shell.Write("注意：2秒后关闭...");</example>
        public static void WriteAttention(string output)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(@"{0}", output);
        }
        #endregion
        /// <summary>
        /// 根据输出文本选择控制台文字颜色
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>  
        private static ConsoleColor GetConsoleColor(string output)
        {
            if (output.StartsWith("警告")) return ConsoleColor.Yellow;
            if (output.StartsWith("错误")) return ConsoleColor.Red;
            if (output.StartsWith("注意")) return ConsoleColor.Green;
            return ConsoleColor.Gray;
        }
    }
}