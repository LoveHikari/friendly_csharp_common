#if !NETSTANDARD2_0
using System.Collections.Generic;
using System.Threading;

/******************************************************************************************************************
 * 
 * 
 * 标  题： string 帮助类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2017/3/30
 * 修  改：
 * 参  考： http://fiddler.wikidot.com/fiddlercore-demo2?qqdrsign=0256c?qqdrsign=00893
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/
namespace System
{
    /// <summary>
    /// 请求拦截
    /// </summary>
    public class FiddlerListeners
    {
        /// <summary>
        /// 响应列表
        /// </summary>
        public List<Fiddler.Session> AllSessions { get; set; }
        /// <summary>
        /// 开始
        /// </summary>
        public void DoStart()
        {
            AllSessions = new List<Fiddler.Session>();
            #region AttachEventListeners
            Fiddler.FiddlerApplication.BeforeRequest += delegate (Fiddler.Session oS) {
                oS.bBufferResponse = true;
                Monitor.Enter(AllSessions);
                AllSessions.Add(oS);
                Monitor.Exit(AllSessions);
            };

            Fiddler.FiddlerApplication.BeforeResponse += delegate (Fiddler.Session oS) {
                Console.WriteLine("{0}:HTTP {1} for {2}", oS.id, oS.responseCode, oS.fullUrl);
                oS.utilDecodeResponse(); oS.utilReplaceInResponse("Microsoft", "Bayden");
            };

            #endregion AttachEventListeners
            Fiddler.CONFIG.IgnoreServerCertErrors = false;

            Fiddler.FiddlerApplication.Startup(8877, true, true, true);
        }
        /// <summary>
        /// 清空响应列表
        /// </summary>
        public void ClearSessions()
        {
            Monitor.Enter(AllSessions);
            AllSessions.Clear();
            Monitor.Exit(AllSessions);
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void DoQuit()
        {
            Console.WriteLine("Shutting down...");
            Fiddler.FiddlerApplication.Shutdown();
        }
    }
}
#endif
