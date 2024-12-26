namespace Hikari.Common;
/// <summary>
/// <see cref="decimal"/> 扩展类
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.Browsable(false)]
public static class DecimalExtensions
{
    #region 阿拉伯数字转中文

    /// <summary>
    /// 获得中文数字
    /// 例：222   ====》二百二十二
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public static string ToChineseNumber(this double d)
    {
        string x = d.ToString();
        if (x.Length == 0)
        {
            return "";
        }
        string text = "";
        if (x[0] == '-')
        {
            text = "负";
            x = x.Remove(0, 1);
        }
        if (x[0].ToString() == ".")
        {
            x = "0" + x;
        }
        if (x[x.Length - 1].ToString() == ".")
        {
            x = x.Remove(x.Length - 1, 1);
        }
        if (x.IndexOf(".") > -1)
        {
            text = text + StrToInt(x.Substring(0, x.IndexOf("."))) + "点" + StrToDouble(x.Substring(x.IndexOf(".") + 1));
        }
        else
        {
            text += StrToInt(x);
        }
        return text;
    }
    /// <summary>
    /// 获得中文大写金额
    /// </summary>
    /// <param name="money"></param>
    /// <returns></returns>
    public static string ToChineseMoney(this decimal money)
    {
        if (money == 0.0M)
        {
            return "";
        }
        string text = money.ToString("#0.000");
        int num = text.IndexOf(".");
        if (num > 0)
        {
            text = text.Replace(".", "");
        }
        if (text.Substring(0, 1) == "0")
        {
            text = text.Remove(0, 1);
        }
        text = NumstrToChinese(text);
        if (text.Length == 0)
        {
            return "";
        }
        if (money < 0.0M)
        {
            text = "负" + text;
        }
        text = text.Replace("0", "零");
        text = text.Replace("1", "壹");
        text = text.Replace("2", "贰");
        text = text.Replace("3", "叁");
        text = text.Replace("4", "肆");
        text = text.Replace("5", "伍");
        text = text.Replace("6", "陆");
        text = text.Replace("7", "柒");
        text = text.Replace("8", "捌");
        text = text.Replace("9", "玖");
        text = text.Replace("M", "亿");
        text = text.Replace("W", "万");
        text = text.Replace("S", "仟");
        text = text.Replace("H", "佰");
        text = text.Replace("T", "拾");
        text = text.Replace("Y", "圆");
        text = text.Replace("J", "角");
        text = text.Replace("F", "分");
        text = text.Replace("L", "厘");
        if (text.Substring(text.Length - 1, 1) != "厘" && text.Substring(text.Length - 1, 1) != "分" && text.Substring(text.Length - 1, 1) != "角")
        {
            text += "整";
        }
        return text;
    }

    private static char CharToNum(char x)
    {
        string text = "零一二三四五六七八九";
        string text2 = "0123456789";
        return text[text2.IndexOf(x)];
    }

    private static string WanStrToInt(string x)
    {
        string[] array = new string[]
        {
            "",
            "十",
            "百",
            "千"
        };
        string text = "";
        int i;
        for (i = x.Length - 1; i >= 0; i--)
        {
            if (x[i] == '0')
            {
                text = CharToNum(x[i]) + text;
            }
            else
            {
                text = CharToNum(x[i]) + array[x.Length - 1 - i] + text;
            }
        }
        while ((i = text.IndexOf("零零")) != -1)
        {
            text = text.Remove(i, 1);
        }
        if (text[text.Length - 1] == '零' && text.Length > 1)
        {
            text = text.Remove(text.Length - 1, 1);
        }
        if (text.Length >= 2 && text.Substring(0, 2) == "一十")
        {
            text = text.Remove(0, 1);
        }
        return text;
    }

    private static string StrToInt(string x)
    {
        int length = x.Length;
        string text;
        if (length <= 4)
        {
            text = WanStrToInt(x);
        }
        else if (length <= 8)
        {
            text = WanStrToInt(x.Substring(0, length - 4)) + "万";
            string text2 = WanStrToInt(x.Substring(length - 4, 4));
            if (text2.IndexOf("千") == -1 && text2 != "")
            {
                text = text + "零" + text2;
            }
            else
            {
                text += text2;
            }
        }
        else
        {
            text = WanStrToInt(x.Substring(0, length - 8)) + "亿";
            string text2 = WanStrToInt(x.Substring(length - 8, 4));
            if (text2.IndexOf("千") == -1 && text2 != "")
            {
                text = text + "零" + text2;
            }
            else
            {
                text += text2;
            }
            text += "万";
            text2 = WanStrToInt(x.Substring(length - 4, 4));
            if (text2.IndexOf("千") == -1 && text2 != "")
            {
                text = text + "零" + text2;
            }
            else
            {
                text += text2;
            }
        }
        int num;
        if ((num = text.IndexOf("零万")) != -1)
        {
            text = text.Remove(num + 1, 1);
        }
        while ((num = text.IndexOf("零零")) != -1)
        {
            text = text.Remove(num, 1);
        }
        if (text[text.Length - 1] == '零' && text.Length > 1)
        {
            text = text.Remove(text.Length - 1, 1);
        }
        return text;
    }

    private static string StrToDouble(string x)
    {
        string text = "";
        for (int i = 0; i < x.Length; i++)
        {
            text += CharToNum(x[i]);
        }
        return text;
    }

    private static string NumstrToChinese(string numstr)
    {
        string[] array = new string[4];
        string text = "";
        bool flag = false;
        array[0] = "";
        array[1] = "T";
        array[2] = "H";
        array[3] = "S";
        for (int i = 1; i <= numstr.Length; i++)
        {
            int num = numstr.Length - i;
            string text2 = numstr.Substring(i - 1, 1);
            if (text2 != "0" && num > 2)
            {
                text = text + text2 + array[(num - 3) % 4];
            }
            if (text2 == "0" && !flag)
            {
                text += "0";
                flag = true;
            }
            if (num == 15)
            {
                if (text.Substring(text.Length - 1) == "0")
                {
                    text = text.Substring(0, text.Length - 1) + "W0";
                }
                else
                {
                    text += "W";
                }
            }
            if (num == 3)
            {
                if (text.Substring(text.Length - 1, 1) == "0")
                {
                    text = text.Substring(0, text.Length - 1) + "Y0";
                }
                else
                {
                    text += "Y";
                }
            }
            if (num == 7)
            {
                if (text.Length > 2)
                {
                    if (text.Substring(text.Length - 2) != "M0")
                    {
                        if (text.Substring(text.Length - 1) == "0")
                        {
                            text = text.Substring(0, text.Length - 1) + "W0";
                        }
                        else
                        {
                            text += "W";
                        }
                    }
                }
                else if (text.Substring(text.Length - 1) == "0")
                {
                    text = text.Substring(0, text.Length - 1) + "W0";
                }
                else
                {
                    text += "W";
                }
            }
            if (num == 11)
            {
                if (text.Substring(text.Length - 1) == "0")
                {
                    text = text.Substring(0, text.Length - 1) + "M0";
                }
                else
                {
                    text += "M";
                }
            }
            if (num == 0 && text2 != "0")
            {
                text = text + text2 + "L";
            }
            if (num == 1 && text2 != "0")
            {
                text = text + text2 + "F";
            }
            if (num == 2 && text2 != "0")
            {
                text = text + text2 + "J";
            }
            if (text2 != "0")
            {
                flag = false;
            }
        }
        if (text.Substring(0, 1) == "1" && text.Substring(1, 1) == array[1])
        {
            text = text.Substring(1);
        }
        if (text.Substring(text.Length - 1, 1) == "0")
        {
            text = text.Substring(0, text.Length - 1);
        }
        if (text.Substring(0, 1) == "0")
        {
            text = text.Substring(1);
        }
        if (text.Substring(text.Length - 1, 1) == "M" || text.Substring(text.Length - 1, 1) == "W" || text.Substring(text.Length - 1, 1) == "S" || text.Substring(text.Length - 1, 1) == "H" || text.Substring(text.Length - 1, 1) == "T")
        {
            text += "Y";
        }
        return text;
    }

    #endregion
    /// <summary>
    /// 将中文金额转换为阿拉伯数字
    /// </summary>
    /// <param name="chineseAmount">中文金额</param>
    /// <returns></returns>
    public static decimal ToArabicNumber(this string chineseAmount)
    {
        Dictionary<char, int> chineseDigits = new Dictionary<char, int>
        {
            { '零', 0 }, { '壹', 1 }, { '贰', 2 }, { '叁', 3 }, { '肆', 4 },
            { '伍', 5 }, { '陆', 6 }, { '柒', 7 }, { '捌', 8 }, { '玖', 9 }
        };

        Dictionary<char, int> chineseUnits = new Dictionary<char, int>
        {
            { '分', 10 }, { '角', 100 }, { '厘', 1 }, { '元', 1 }, { '圆', 1 }, { '拾', 10 }, { '百', 100 }, { '佰', 100 },
            { '千', 1000 }, { '仟', 1000 },
            { '万', 10000 }, { '萬', 10000 }, { '亿', 100000000 }
        };
        decimal result = 0;
        decimal resultTemp = 0;
        decimal currentUnitValue = 0;
        bool isLastCharDigit = false;

        foreach (char c in chineseAmount)
        {
            if (chineseDigits.TryGetValue(c, out int digit))
            {
                currentUnitValue += digit;
                isLastCharDigit = true;
            }
            else if (chineseUnits.TryGetValue(c, out int unit))
            {
                if (unit == 10000 || unit == 100000000) // 处理“万”和“亿”
                {
                    result -= resultTemp;
                    result += (resultTemp + currentUnitValue) * unit;
                    currentUnitValue = 0; // 重置当前单位值
                    resultTemp = 0;
                }
                else
                if (isLastCharDigit)
                {
                    currentUnitValue *= unit;
                    result += currentUnitValue;
                    resultTemp += currentUnitValue;
                    currentUnitValue = 0;
                }
                else if (c == '零')
                {
                    // 如果是零，直接跳过
                    continue;
                }
                if (c == '圆' || c == '元')
                {
                    result *= 1000;
                }
                isLastCharDigit = false;
            }
        }

        // 处理最后的数字
        result += currentUnitValue;

        return result / 1000; // 返回以元为单位
    }
}
