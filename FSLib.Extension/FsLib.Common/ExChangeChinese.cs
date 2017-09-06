#if !NETSTANDARD2_0
using System.Reflection;

/******************************************************************************************************************
 * 
 * 
 * 标  题： 中文处理(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2016/11/18
 * 修  改：
 * 参  考： http://blog.csdn.net/marvinhong/article/details/3511347
 * 说  明： 需要引用com组件Microsoft.Office.Interop.Word.dll
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/

namespace System
{
    /// <summary>
    /// 中文处理
    /// </summary>
    public class ExChangeChinese
    {
        /// <summary>
        /// 简体转繁体
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Simplified2Traditional(string src)
        {
            string des = "";
            Microsoft.Office.Interop.Word._Application appWord = new Microsoft.Office.Interop.Word.Application();
            object template = Missing.Value;
            object newTemplate = Missing.Value;
            object docType = Missing.Value;
            object visible = true;
            Microsoft.Office.Interop.Word.Document doc = appWord.Documents.Add(ref template, ref newTemplate, ref docType, ref visible);
            appWord.Selection.TypeText(src);
            appWord.Selection.Range.TCSCConverter(Microsoft.Office.Interop.Word.WdTCSCConverterDirection.wdTCSCConverterDirectionSCTC, true, true);
            appWord.ActiveDocument.Select();
            des = appWord.Selection.Text;
            object saveChange = 0;
            object originalFormat = Missing.Value;
            object routeDocument = Missing.Value;
            appWord.Quit(ref saveChange, ref originalFormat, ref routeDocument);
            doc = null;
            appWord = null;
            GC.Collect();//进程资源释放
            return des;
        }
        /// <summary>
        /// 繁体转简体
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Traditional2Simplified(string src)
        {
            string des = "";
            Microsoft.Office.Interop.Word._Application appWord = new Microsoft.Office.Interop.Word.Application();
            object template = Missing.Value;
            object newTemplate = Missing.Value;
            object docType = Missing.Value;
            object visible = true;
            Microsoft.Office.Interop.Word.Document doc = appWord.Documents.Add(ref template, ref newTemplate, ref docType, ref visible);
            appWord.Selection.TypeText(src);
            appWord.Selection.Range.TCSCConverter(Microsoft.Office.Interop.Word.WdTCSCConverterDirection.wdTCSCConverterDirectionTCSC, true, true);
            appWord.ActiveDocument.Select();
            des = appWord.Selection.Text;
            object saveChange = 0;
            object originalFormat = Missing.Value;
            object routeDocument = Missing.Value;
            appWord.Quit(ref saveChange, ref originalFormat, ref routeDocument);
            doc = null;
            appWord = null;
            GC.Collect();//进程资源释放
            return des;
        }

    }
}
#endif
