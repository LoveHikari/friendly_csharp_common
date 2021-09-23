/***
 * title:中文大写类
 * date:2016-5-31
 * author:YUXiaoWei
 ***/

using System;

namespace Hikari.Common
{
    /// <summary>
    /// 转中文数字
    /// </summary>
    public class ChineseNum
    {
        /// <summary>
        /// 获得中文数字
        /// 例：222   ====》二百二十二
        /// </summary>
        /// <param name="p_num"></param>
        /// <returns></returns>
        public static string GetChineseNum(string p_num)
        {
            ChineseNum chineseNum = new ChineseNum();
            return chineseNum.NumToChn(p_num);
        }
        /// <summary>
        /// 获得中文大写金额
        /// </summary>
        /// <param name="p_Money"></param>
        /// <returns></returns>
        public static string GetUpperMoney(double p_Money)
        {
            ChineseNum chineseNum = new ChineseNum();
            return chineseNum.GetMoneyChinese(p_Money);
        }

        private char CharToNum(char x)
        {
            string text = "零一二三四五六七八九";
            string text2 = "0123456789";
            return text[text2.IndexOf(x)];
        }

        private string WanStrToInt(string x)
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
                    text = this.CharToNum(x[i]) + text;
                }
                else
                {
                    text = this.CharToNum(x[i]) + array[x.Length - 1 - i] + text;
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

        private string StrToInt(string x)
        {
            int length = x.Length;
            string text;
            if (length <= 4)
            {
                text = this.WanStrToInt(x);
            }
            else if (length <= 8)
            {
                text = this.WanStrToInt(x.Substring(0, length - 4)) + "万";
                string text2 = this.WanStrToInt(x.Substring(length - 4, 4));
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
                text = this.WanStrToInt(x.Substring(0, length - 8)) + "亿";
                string text2 = this.WanStrToInt(x.Substring(length - 8, 4));
                if (text2.IndexOf("千") == -1 && text2 != "")
                {
                    text = text + "零" + text2;
                }
                else
                {
                    text += text2;
                }
                text += "万";
                text2 = this.WanStrToInt(x.Substring(length - 4, 4));
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

        private string StrToDouble(string x)
        {
            string text = "";
            for (int i = 0; i < x.Length; i++)
            {
                text += this.CharToNum(x[i]);
            }
            return text;
        }

        private string NumToChn(string x)
        {
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
                text = text + this.StrToInt(x.Substring(0, x.IndexOf("."))) + "点" + this.StrToDouble(x.Substring(x.IndexOf(".") + 1));
            }
            else
            {
                text += this.StrToInt(x);
            }
            return text;
        }

        private string GetMoneyChinese(double Money)
        {
            if (Money == 0.0)
            {
                return "";
            }
            string text = Money.ToString("#0.00");
            int num = text.IndexOf(".");
            if (num > 0)
            {
                text = text.Replace(".", "");
            }
            if (text.Substring(0, 1) == "0")
            {
                text = text.Remove(0, 1);
            }
            text = this.NumstrToChinese(text);
            if (text.Length == 0)
            {
                return "";
            }
            if (Money < 0.0)
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
            if (text.Substring(text.Length - 1, 1) != "分")
            {
                text += "整";
            }
            return text;
        }

        private string NumstrToChinese(string numstr)
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
                if (text2 != "0" && num > 1)
                {
                    text = text + text2 + array[(num - 2) % 4];
                }
                if (text2 == "0" && !flag)
                {
                    text += "0";
                    flag = true;
                }
                if (num == 14)
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
                if (num == 2)
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
                if (num == 6)
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
                if (num == 10)
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
                    text = text + text2 + "F";
                }
                if (num == 1 && text2 != "0")
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

        #region 阿拉伯数字转换成大写
        /// 转换数字金额主函数（包括小数） 
        /// 
        /// 数字字符串 
        /// 转换成中文大写后的字符串或者出错信息提示字符串 
        public static string ConvertSum(string str)
        {
            if (!IsPositveDecimal(str))
                return "输入的不是正数字！";
            if (Double.Parse(str) > 999999999999.99)
                return "数字太大，无法换算，请输入一万亿元以下的金额";
            char[] ch = new char[1];
            ch[0] = '.'; //小数点 
            string[] splitstr = str.Split(ch[0]);// 定义按小数点分割后的字符串数组, 按小数点分割字符串 
            if (splitstr.Length == 1) //只有整数部分 
                return ConvertData(str) + "圆整";
            else //有小数部分 
            {
                string rstr = ConvertData(splitstr[0]) + "圆";
                rstr += ConvertXiaoShu(splitstr[1]);//转换小数部分 
                return rstr;
            }

        }

        /// 判断是否是正数字字符串 
        /// 
        /// 判断字符串 
        /// 如果是数字，返回true，否则返回false 
        public static bool IsPositveDecimal(string str)
        {
            Decimal d;
            try
            {
                d = Decimal.Parse(str);

            }
            catch (Exception)
            {
                return false;
            }
            if (d > 0)
                return true;
            else
                return false;
        }
        /// 
        /// 转换数字（整数） 
        /// 
        /// 需要转换的整数数字字符串 
        /// 转换成中文大写后的字符串 
        public static string ConvertData(string str)
        {
            string tmpstr = "";
            string rstr = "";
            int strlen = str.Length;
            if (strlen <= 4)//数字长度小于四位 
            {
                rstr = ConvertDigit(str);

            }
            else
            {

                if (strlen <= 8)//数字长度大于四位，小于八位 
                {
                    tmpstr = str.Substring(strlen - 4, 4);//先截取最后四位数字 
                    rstr = ConvertDigit(tmpstr);//转换最后四位数字 
                    tmpstr = str.Substring(0, strlen - 4);//截取其余数字 
                    //将两次转换的数字加上萬后相连接 
                    rstr = String.Concat(ConvertDigit(tmpstr) + "萬", rstr);
                    rstr = rstr.Replace("零萬", "萬");
                    rstr = rstr.Replace("零零", "零");

                }
                else
                    if (strlen <= 12)//数字长度大于八位，小于十二位 
                {
                    tmpstr = str.Substring(strlen - 4, 4);//先截取最后四位数字 
                    rstr = ConvertDigit(tmpstr);//转换最后四位数字 
                    tmpstr = str.Substring(strlen - 8, 4);//再截取四位数字 
                    rstr = String.Concat(ConvertDigit(tmpstr) + "萬", rstr);
                    tmpstr = str.Substring(0, strlen - 8);
                    rstr = String.Concat(ConvertDigit(tmpstr) + "億", rstr);
                    rstr = rstr.Replace("零億", "億");
                    rstr = rstr.Replace("零萬", "零");
                    rstr = rstr.Replace("零零", "零");
                    rstr = rstr.Replace("零零", "零");
                }
            }
            strlen = rstr.Length;
            if (strlen >= 2)
            {
                switch (rstr.Substring(strlen - 2, 2))
                {
                    case "佰零": rstr = rstr.Substring(0, strlen - 2) + "佰"; break;
                    case "仟零": rstr = rstr.Substring(0, strlen - 2) + "仟"; break;
                    case "萬零": rstr = rstr.Substring(0, strlen - 2) + "萬"; break;
                    case "億零": rstr = rstr.Substring(0, strlen - 2) + "億"; break;

                }
            }

            return rstr;
        }

        /// 转换数字（小数部分） 
        /// 
        /// 需要转换的小数部分数字字符串 
        /// 转换成中文大写后的字符串 
        public static string ConvertXiaoShu(string str)
        {
            int strlen = str.Length;
            string rstr;
            if (strlen == 1)
            {
                rstr = ConvertChinese(str) + "角";
                return rstr;
            }
            else
            {
                string tmpstr = str.Substring(0, 1);
                rstr = ConvertChinese(tmpstr) + "角";
                tmpstr = str.Substring(1, 1);
                rstr += ConvertChinese(tmpstr) + "分";
                rstr = rstr.Replace("零分", "");
                rstr = rstr.Replace("零角", "");
                return rstr;
            }

        }

        /// 
        /// 转换数字 
        /// 
        /// 转换的字符串（四位以内） 
        /// 
        public static string ConvertDigit(string str)
        {
            int strlen = str.Length;
            string rstr = "";
            switch (strlen)
            {
                case 1: rstr = ConvertChinese(str); break;
                case 2: rstr = Convert2Digit(str); break;
                case 3: rstr = Convert3Digit(str); break;
                case 4: rstr = Convert4Digit(str); break;
            }
            rstr = rstr.Replace("拾零", "拾");
            strlen = rstr.Length;

            return rstr;
        }

        /// 
        /// 转换四位数字 
        /// 
        public static string Convert4Digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string str3 = str.Substring(2, 1);
            string str4 = str.Substring(3, 1);
            string rstring = "";
            rstring += ConvertChinese(str1) + "仟";
            rstring += ConvertChinese(str2) + "佰";
            rstring += ConvertChinese(str3) + "拾";
            rstring += ConvertChinese(str4);
            rstring = rstring.Replace("零仟", "零");
            rstring = rstring.Replace("零佰", "零");
            rstring = rstring.Replace("零拾", "零");
            rstring = rstring.Replace("零零", "零");
            rstring = rstring.Replace("零零", "零");
            rstring = rstring.Replace("零零", "零");
            return rstring;
        }
        /// 
        /// 转换三位数字 
        /// 
        public static string Convert3Digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string str3 = str.Substring(2, 1);
            string rstring = "";
            rstring += ConvertChinese(str1) + "佰";
            rstring += ConvertChinese(str2) + "拾";
            rstring += ConvertChinese(str3);
            rstring = rstring.Replace("零佰", "零");
            rstring = rstring.Replace("零拾", "零");
            rstring = rstring.Replace("零零", "零");
            rstring = rstring.Replace("零零", "零");
            return rstring;
        }
        /// 
        /// 转换二位数字 
        /// 
        public static string Convert2Digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string rstring = "";
            rstring += ConvertChinese(str1) + "拾";
            rstring += ConvertChinese(str2);
            rstring = rstring.Replace("零拾", "零");
            rstring = rstring.Replace("零零", "零");
            return rstring;
        }
        /// <summary>
        /// 转中文数字
        /// </summary>
        /// <param name="str">0~9</param>
        /// <returns></returns>
        public static string ConvertChinese(string str)
        {
            //"零壹贰叁肆伍陆柒捌玖拾佰仟萬億圆整角分" 
            string cstr = "";
            switch (str)
            {
                case "0": cstr = "零"; break;
                case "1": cstr = "壹"; break;
                case "2": cstr = "贰"; break;
                case "3": cstr = "叁"; break;
                case "4": cstr = "肆"; break;
                case "5": cstr = "伍"; break;
                case "6": cstr = "陆"; break;
                case "7": cstr = "柒"; break;
                case "8": cstr = "捌"; break;
                case "9": cstr = "玖"; break;

            }
            return cstr;
        }


        #endregion
    }
}
