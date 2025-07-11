﻿using System.Text;

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
    /// StringBuilder 扩展类
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// 向此实例追加指定字符串的副本。
        /// </summary>
        /// <param name="this"></param>
        /// <param name="spaceNum">空格数</param>
        /// <param name="text">要追加的字符串</param>
        /// <returns></returns>
        public static StringBuilder AppendSpace(this StringBuilder @this, int spaceNum, string text)
        {
            @this.Append(StringHelper.Space(spaceNum));
            @this.Append(text);
            return @this;
        }
        /// <summary>
        /// 将后面跟有默认行终止符的指定字符串的副本追加到当前 StringBuilder 对象的末尾
        /// </summary>
        /// <param name="this"></param>
        /// <param name="spaceNum">空格数</param>
        /// <param name="text">要追加的字符串</param>
        /// <returns></returns>
        public static StringBuilder AppendSpaceLine(this StringBuilder @this, int spaceNum, string text)
        {
            @this.Append(StringHelper.Space(spaceNum));
            @this.AppendLine(text);
            return @this;
        }
        /// <summary>
        /// 将后面跟有默认行终止符的指定字符串的副本追加到当前 StringBuilder 对象的末尾
        /// </summary>
        /// <param name="this"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static StringBuilder AppendFormatLine(this StringBuilder @this, string format, params object[] args)
        {
            @this.AppendFormat(format, args);
            @this.AppendLine();
            return @this;
        }
       
    }
}