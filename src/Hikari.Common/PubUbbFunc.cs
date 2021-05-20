using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

/******************************************************************************************************************
 * 
 * 
 * 标  题：html与ubb(版本：Version1.0.0)
 * 作  者：YuXiaoWei
 * 日  期：2016/10/20
 * 修  改：
 * 参  考： 
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common
{
    /// <summary>
    /// html与ubb
    /// </summary>
    public class PubUbbFunc
    {  
        /// <summary>
        /// 多行文本框前台页面HMTL显示
        /// </summary>
        /// <param name="sDetail"></param>
        /// <returns></returns>
        public static string TextBoxToHtml(string sDetail)
        {
            sDetail = sDetail.Replace(" ", "&nbsp;").Replace("　", "&nbsp;");
            sDetail = sDetail.Replace("'", "&#39;");
            sDetail = sDetail.Replace("\"", "&quot;");
            sDetail = sDetail.Replace("<", "&lt;");
            sDetail = sDetail.Replace(">", "&gt;");
            sDetail = sDetail.Replace("\r\n", "<BR>");
            return sDetail;
        }
        /// <summary>
        /// 前台页面多行文本框显示
        /// </summary>
        /// <param name="sDetail"></param>
        /// <returns></returns>
        public static string HtmlToTextBox(string sDetail)
        {
            sDetail = sDetail.Replace("&nbsp;", " ").Replace("&nbsp;", "　");
            sDetail = sDetail.Replace("&#39;","'");
            sDetail = sDetail.Replace("&quot;","\"");
            sDetail = sDetail.Replace("&lt;","<");
            sDetail = sDetail.Replace("&gt;",">");
            sDetail = sDetail.Replace("<BR>","\r\n");
            return sDetail;
        }

        /// <summary>
        /// 取得HTML中所有图片的 URL。
        /// </summary>
        /// <param name="sHtmlText">HTML代码</param>
        /// <returns>图片的URL列表</returns>
        public static List<string> GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串
            MatchCollection matches = regImg.Matches(sHtmlText);

            List<string> sUrlList = new List<string>();

            // 取得匹配项列表
            foreach (Match match in matches)
                sUrlList.Add(match.Groups["imgUrl"].Value);

            return sUrlList;
        }

        #region 替换字符&->&amp;
        /// <summary>
        /// html特殊字符转义
        /// </summary>
        /// <param name="data"></param>
        /// <returns>转义字符</returns>
        public static string ReplaceChar(string data)
        {
            if (data == null) return string.Empty;
            StringBuilder sbString = new StringBuilder(data, data.Length * 2);
            sbString.Replace("&", "&amp;");
            sbString.Replace("<", "&lt;");
            sbString.Replace(">", "&gt;");
            sbString.Replace("'", "&apos;");
            sbString.Replace("\"", "&quot;");

            return sbString.ToString();
        }
        /// <summary>
        /// html转义字符转特殊字符
        /// </summary>
        /// <param name="data"></param>
        /// <returns>特殊字符</returns>
        public static string ReplaceCharBack(string data)
        {
            if (data == null) return string.Empty;
            StringBuilder sbString = new StringBuilder(data, data.Length * 2);
            sbString.Replace("&amp;", "&");
            sbString.Replace("&lt;", "<");
            sbString.Replace("&gt;", ">");
            sbString.Replace("&apos;", "'");
            sbString.Replace("&quot;", "\"");
            sbString.Replace("\n", "");
            sbString.Replace("\r", "");
            return sbString.ToString();
        }
        #endregion
    }
}
