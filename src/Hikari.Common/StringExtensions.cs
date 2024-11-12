using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace Hikari.Common
{
    /// <summary>
    /// <see cref="string"/> 扩展方法
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
    public static class StringExtensions
    {
        /// <summary>
        /// 截取指定字节长度的字符串
        /// </summary>
        /// <param name="this">被处理的字符串</param>
        /// <param name="len">要求截取的字节长度</param>
        /// <param name="flag">截取后是否显示省略号,flag为true显示…，为false不显示，默认不显示</param>
        /// <returns></returns>
        public static string CutString(this string @this, int len, bool flag = false)
        {
            //对@this截取len字节的字符
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            string outputString = string.Empty;
            byte[] str = ascii.GetBytes(@this);
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
                try
                {
                    outputString += @this.Substring(i, 1);
                }
                catch
                {
                    break;
                }
                if (tempLen >= len)
                    break;
            }

            //如果截过则加上半个省略号
            if (@this != outputString && flag)
                outputString += "…";
            return outputString;
        }

        /// <summary>
        /// 过滤Sql查询关键词中的敏感词汇
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SqlFilter(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            Regex regsql = new Regex(@"0x([0-9a-fA-F]{4})+|(%[0-9a-fA-F]{2})+|--|@@|count|asc|mid|char|chr|sysobjects|sys.|select|insert|delete|update|drop|truncate|xp_cmdshell|netlocalgroup|administrator|net user|exec|master|declare|localgroup|remove|create|extended_properties|objects|columns|types|extended|comments|table|cast", RegexOptions.IgnoreCase);

            value = value.Replace("'", "''").Trim();
            //捕获的字符转换为""
            return regsql.Replace(value, m => string.Empty);
        }

        /// <summary>
        /// 获取左边指定位数的字符串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string SubLeft(this string? value, int length)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            if (value.Length <= length)
            {
                return value;
            }
            return value.Substring(0, length);
        }
        /// <summary>
        /// 获取右边指定位数的字符串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string SubRight(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            if (value.Length <= length)
            {
                return value;
            }

            return value.Substring(value.Length - length);
        }
        /// <summary>
        /// 获取拆分符左边的字符串，不包括拆分符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="split">拆分符</param>
        /// <returns></returns>
        public static string SplitLeft(this string value, string split)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            int index = value.IndexOf(split, StringComparison.Ordinal);
            if (index > 0)
            {
                return value.Substring(0, index);
            }
            else
            {
                return value;
            }
        }
        /// <summary>
        /// 获取拆分符右边的字符串，不包括拆分符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="split">拆分符</param>
        /// <returns></returns>
        public static string SplitRight(this string value, string split)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            int index = value.IndexOf(split, StringComparison.Ordinal);
            if (index > 0)
            {
                return value.Substring(index + split.Length);
            }
            else
            {
                return value;
            }
        }
        /// <summary>
        /// 获取序号之间的字符串，包括两端序号，序号从0开始
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex">开始序号</param>
        /// <param name="endIndex">结束序号</param>
        /// <returns></returns>
        public static string SubBetween(this string value, int startIndex, int endIndex)
        {
            value = value.Substring(startIndex, endIndex - startIndex + 1);
            return value;
        }
        /// <summary>
        /// 删除不可见字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveEmpty(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            Regex reg = new Regex(@"[\f\n\r\t\v]*", RegexOptions.IgnoreCase);
            value = reg.Replace(value, "");
            reg = new Regex(@"[ ]+");//合并多个空格为一个
            return reg.Replace(value, " ");
        }
        /// <summary>
        /// 从右边开始去掉i个字符
        /// </summary>
        /// <param name="this"></param>
        /// <param name="i">要去掉的长度</param>
        /// <returns></returns>
        public static string RemoveRight(this string @this, int i)
        {
            return @this[..^i];
            //return @this.Remove(@this.Length - i, i);
        }
        /// <summary>
        /// 过滤文本中的空行
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveEmptyRow(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            Regex reg = new Regex(@"\n[\t|\s| ]*\r", RegexOptions.IgnoreCase);
            return reg.Replace(value, "");
        }

        /// <summary>
        /// 获取字符串在数组中累计出现的次数
        /// </summary>
        /// <param name="stringArray"></param>
        /// <param name="findString"></param>
        /// <returns></returns>
        public static int GetStringCount(this string[] stringArray, string findString)
        {
            string totalString = string.Join("", stringArray);
            return GetStringCount(totalString, findString);
        }

        /// <summary>
        /// 获取字符串在字符串累计出现的次数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="findString"></param>
        /// <returns></returns>
        public static int GetStringCount(this string value, string findString)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            int count = 0;
            int findStringLength = findString.Length;
            string subString = value;

            while (subString.IndexOf(findString, StringComparison.Ordinal) >= 0)
            {
                subString = subString.Substring(subString.IndexOf(findString, StringComparison.Ordinal) + findStringLength);
                count += 1;
            }
            return count;
        }
        /// <summary>
        /// 截取从startString开始到结尾的字符，不包括前端
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startString"></param>
        /// <returns></returns>
        public static string Substring(this string value, string startString)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            int index = value.IndexOf(startString, StringComparison.Ordinal);
            if (index == -1) return "";
            index += startString.Length;
            if (index > 0)
            {
                return value.Substring(index);
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 截取从startString开始到endString的字符，不包括两端
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startString"></param>
        /// <param name="endString"></param>
        /// <returns></returns>
        public static string Substring(this string value, string startString, string endString)
        {
            value = Substring(value, startString);
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            int length = value.IndexOf(endString, StringComparison.Ordinal);
            if (length > 0)
            {
                return value.Substring(0, length);
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 从当前字符串删除字符串的所有尾随匹配项
        /// </summary>
        /// <param name="this"></param>
        /// <param name="trimStr">要删除的字符串</param>
        /// <returns></returns>
        public static string TrimEnd(this string @this, string trimStr)
        {
            int i = trimStr.Length;
        a:
            string endStr = @this.SubRight(i);
            if (endStr != trimStr) return @this;
            @this = @this.Substring(0, @this.Length - i);
            goto a;
        }
        /// <summary>
        /// 从当前字符串删除字符串的所有前导匹配项
        /// </summary>
        /// <param name="this"></param>
        /// <param name="trimStr">要删除的字符串</param>
        /// <returns></returns>
        public static string TrimStart(this string @this, string trimStr)
        {
            int i = trimStr.Length;
        a:
            string startStr = @this.SubLeft(i);
            if (startStr != trimStr) return @this;
            @this = @this.Remove(0, i);
            goto a;
        }
        /// <summary>
        /// 从当前字符串删除字符串的所有前导匹配项和尾随匹配项
        /// </summary>
        /// <param name="this"></param>
        /// <param name="trimStr">要删除的字符串</param>
        /// <returns></returns>
        public static string Trim(this string @this, string trimStr)
        {
            return @this.TrimStart(trimStr).TrimEnd(trimStr);
        }

        #region 全角半角转换
        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToSbc(this string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }


        /// <summary> 转半角的函数(DBC case) </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToDbc(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        #endregion

        #region 类型转换

        /// <summary>
        /// 提取出所有数字，并转换为int，失败返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToNumber(this string? value)
        {
            if (value == null)
                return 0;
            string num = "";
            foreach (char item in value)
            {
                if (item >= 48 && item <= 58)
                {
                    num += item;
                }
            }

            return int.TryParse(num, out var result) ? result : 0;
        }

        /// <summary>
        /// 如果字符串是数，则进行四舍五入，否则不做任何操作;如果是整数则返回整数，否则返回<paramref name="digits"/>位小数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="digits">小数位数</param>
        /// <returns></returns>
        public static string Round(this string s, int digits)
        {
            double result;
            if (double.TryParse(s, out result))  //如果是数字，则保留两位小数
            {
                result = Math.Round(result, digits);
                if (Int32.TryParse(result.ToString(CultureInfo.InvariantCulture), out _))
                {
                    return result.ToString(CultureInfo.InvariantCulture);
                }
                string format = "{0:F" + digits + "}";
                return string.Format(format, result);
            }
            return s;
        }
        #endregion

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <param name="culture">一个对象，用于提供区域性特定的大小写规则</param>
        /// <returns></returns>
        public static string ToFirstUpper(this string str, CultureInfo culture = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (culture == null)
            {
                str = str.Substring(0, 1).ToUpper() + str.Substring(1);
            }
            else
            {
                str = str.Substring(0, 1).ToUpper(culture) + str.Substring(1);
            }
            return str;
            //s = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s)
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="str"></param>
        /// <param name="culture">一个对象，用于提供区域性特定的大小写规则</param>
        /// <returns></returns>
        public static string ToFirstLower(this string str, CultureInfo culture = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (culture == null)
            {
                str = str.Substring(0, 1).ToLower() + str.Substring(1);
            }
            else
            {
                str = str.Substring(0, 1).ToLower(culture) + str.Substring(1);
            }
            return str;
        }
        /// <summary>
        /// 将指定的位置的替换为密码字符，包含两端字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex">开始下标，从0开始</param>
        /// <param name="endIndex">结束下标</param>
        /// <param name="passwordValue">密码字符</param>
        /// <returns>替换后的字符串</returns>
        public static string HideChar(this string str, int startIndex, int endIndex, string passwordValue = "*")
        {
            string nv = passwordValue;
            for (int i = 0; i < endIndex - startIndex; i++)
            {
                nv += nv;
            }
            return str.Replace(str.SubBetween(startIndex, endIndex), nv);
        }

        /// <summary>
        /// 转换成时间类型
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this)) return null;

            return DateTime.TryParse(@this, out var result) ? result : null;
        }
        /// <summary>
        /// 转换成时间类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="value">失败时间</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string @this, DateTime value)
        {
            if (string.IsNullOrWhiteSpace(@this)) return value;

            return DateTime.TryParse(@this, out var result) ? result : value;
        }
        /// <summary>
        /// 转换成日期类型
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateOnly? ToDateOnly(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this)) return null;
            return DateTime.TryParse(@this, out var result) ? DateOnly.FromDateTime(result) : null;
        }
        /// <summary>
        /// 转换成日期类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="value">失败时间</param>
        /// <returns></returns>
        public static DateOnly ToDateOnly(this string @this, DateOnly value)
        {
            if (string.IsNullOrWhiteSpace(@this)) return value;
            return DateTime.TryParse(@this, out var result) ? DateOnly.FromDateTime(result) : value;
        }
        /// <summary>
        /// 转换成帕斯卡命名
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (value.IndexOf('_') >= 0)
            {
                var parts = value.Split('_');
                var sb = new StringBuilder();
                foreach (var part in parts)
                {
                    if (string.IsNullOrEmpty(part))
                        continue;
                    var str = SingleCamelCase(part);
                    sb.Append(char.ToUpper(str[0]) + str.Substring(1, str.Length - 1));
                }

                return sb.ToString();
            }

            var camelCase = SingleCamelCase(value);
            return char.ToUpper(camelCase[0]) + camelCase.Substring(1, camelCase.Length - 1);
        }
        /// <summary>
        /// 转换成驼峰命名
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string value)
        {
            var pascalCase = value.ToPascalCase();
            return char.ToLower(pascalCase[0]) + pascalCase.Substring(1, pascalCase.Length - 1);
        }
        /// <summary>
        /// 一个单词转换成驼峰命名
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string SingleCamelCase(string value)
        {
            int lowerCaseOffset = 'a' - 'A';
            if (string.IsNullOrEmpty(value))
                return value;

            var len = value.Length;
            var newValue = new char[len];
            var firstPart = true;

            for (var i = 0; i < len; ++i)
            {
                var c0 = value[i];
                var c1 = i < len - 1 ? value[i + 1] : 'A';
                var c0isUpper = c0 is >= 'A' and <= 'Z';
                var c1isUpper = c1 is >= 'A' and <= 'Z';

                if (firstPart && c0isUpper && (c1isUpper || i == 0))
                    c0 = (char)(c0 + lowerCaseOffset);
                else
                    firstPart = false;

                newValue[i] = c0;
            }

            return new string(newValue);
        }
        /// <summary>
        /// 二进制转十进制
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BigInteger BinaryToBigInteger(this string value)
        {
            BigInteger decimalValue = BigInteger.Zero;
            int sign = value[0] == '1' ? -1 : 1;
            if (sign == -1)
            {
                foreach (var t in value)
                {
                    decimalValue <<= 1;
                    decimalValue += t - '0';
                }

                decimalValue--;

                string binary = decimalValue.ToBase(2).TrimStart('0');  // 反码

                List<string> ss = new List<string>();
                foreach (var c in binary)
                {
                    ss.Add(c.ToString() == "0" ? "1" : "0");
                }

                value = string.Join("", ss);

            }

            decimalValue = BigInteger.Zero;

            for (int i = 1; i < value.Length; i++)
            {
                int bit = value[i] - '0';
                decimalValue = (decimalValue << 1) + bit;
            }

            return decimalValue * sign;

        }
    }
}