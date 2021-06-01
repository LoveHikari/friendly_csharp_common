using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

/******************************************************************************************************************
 * 
 * 
 * 标  题： string 帮助类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2016/11/18
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
    /// <see cref="String"/> 帮助类
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 查找两个字符串最长的公共子串(LongestCommonSequence)
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static string Lcs(string value1, string value2)
        {
            if (string.IsNullOrEmpty(value1)
                || string.IsNullOrEmpty(value2))
            {
                return "";
            }
            int[,] d = new int[value1.Length, value2.Length];
            int index = 0;
            int length = 0;

            for (int i = 0; i < value1.Length; i++)
            {
                for (int j = 0; j < value2.Length; j++)
                {
                    // 左上角值
                    int n = i - 1 >= 0 && j - 1 >= 0 ? d[i - 1, j - 1] : 0;
                    // 当前节点值 = "1 + 左上角值" : "0"
                    d[i, j] = value1[i] == value2[j] ? 1 + n : 0;
                    // 如果是最大值，则记录该值和行号
                    if (d[i, j] > length)
                    {
                        length = d[i, j];
                        index = i;
                    }
                }
            }
            return value1.Substring(index - length + 1, length);
        }
        /// <summary>
        /// 空格数量
        /// </summary>
        /// <param name="spaceNum">空格数量</param>
        /// <returns></returns>
        public static string Space(int spaceNum)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < spaceNum; i++)
                stringBuilder.Append(" ");
            return stringBuilder.ToString();
        }
    }
}