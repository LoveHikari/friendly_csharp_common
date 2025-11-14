using System.Text;

/******************************************************************************************************************
 * 
 * 
 * 标  题： StringBuilder 扩展类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2017/01/10
 * 修  改：
 * 参  考： 
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common
{
    /// <summary>
    /// <see cref="System.Text.StringBuilder"/> 扩展
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class StringBuilderExtensions
    {   /// <summary>
        /// <see cref="System.Text.StringBuilder"/> 扩展
        /// </summary>
        /// <param name="this"></param>
        extension(StringBuilder @this)
        {
            /// <summary>
            /// 向此实例追加指定字符串的副本。
            /// </summary>
            /// <param name="spaceNum">空格数</param>
            /// <param name="text">要追加的字符串</param>
            /// <returns></returns>
            public StringBuilder AppendSpace(int spaceNum, string text)
            {
                @this.Append(StringHelper.Space(spaceNum));
                @this.Append(text);
                return @this;
            }

            /// <summary>
            /// 将后面跟有默认行终止符的指定字符串的副本追加到当前 StringBuilder 对象的末尾
            /// </summary>
            /// <param name="spaceNum">空格数</param>
            /// <param name="text">要追加的字符串</param>
            /// <returns></returns>
            public StringBuilder AppendSpaceLine(int spaceNum, string text)
            {
                @this.Append(StringHelper.Space(spaceNum));
                @this.AppendLine(text);
                return @this;
            }

            /// <summary>
            /// 将后面跟有默认行终止符的指定字符串的副本追加到当前 StringBuilder 对象的末尾
            /// </summary>
            /// <param name="format"></param>
            /// <param name="args"></param>
            /// <returns></returns>
            public StringBuilder AppendFormatLine(string format, params object[] args)
            {
                @this.AppendFormat(format, args);
                @this.AppendLine();
                return @this;
            }
        }
    }
}