using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

/******************************************************************************************************************
 * 
 * 
 * 说　明： 计算圆周率(版本：Version1.0.0)
 * 作　者：YuXiaoWei
 * 日　期：2016/10/09
 * 修　改：
 * 参　考：http://www.cnblogs.com/skyivben/archive/2005/09/30/247225.html
 * 备　注：利用圆周率的反正切展式的泰勒级数来计算
 * 
 * 
 * ***************************************************************************************************************/
/*
调用说明：static void Main(string[] args)
       {
           args = new[] {"1"};
           TextWriter tw = Console.Out;
           try
           {
               Pi pi = new Pi();
               if (args.Length < 1)
               {
                   pi.Out(tw);
                   tw.WriteLine("Usage: pi formula [ digits ]");
                   return;
               }
               int formula = Convert.ToInt32(args[0]);
               int digits = 1000;
               if (args.Length > 1) digits = Convert.ToInt32(args[1]);
               if (digits < 0) digits = 0;
               pi.Out(tw, formula - 1, digits);
           }
           catch (Exception ex)
           {
               tw.WriteLine(ex.Message);
           }
       }
*/
namespace Hikari.Common
{
    // 用反正切展式计算圆周率 
    // 例如: pi= + 16 * arctan(1/5) - 4 * arctan(1/239) [Machin] 
    sealed class ThePi
    {
        // 表示 (positive ? + : -) coefficient * arctan(1/denominator) 
        public sealed class Term
        {
            bool positive;   // 系数是否正数 
            int coefficient; // 系数 
            int denominator; // 分母 

            public bool Positive { get { return positive; } }
            public int Coefficient { get { return coefficient; } }
            public int Denominator { get { return denominator; } }

            public Term(bool positive, int coefficient, int denominator)
            {
                this.positive = positive;
                this.coefficient = coefficient;
                this.denominator = denominator;
            }

            public override string ToString()
            {
                return string.Format("{0} {1} * arctan(1/{2}) ", positive ? "+" : "-", coefficient, denominator);
            }
        }

        const int overDigits = 3;           // 额外计算的位数，以消除误差 
        List<Term> list = new List<Term>(); // 反正切展式 
        string name;                        // 反正切展式的发现者 

        public ThePi(string name)
        {
            this.name = name;
        }

        public void Add(Term term)
        {
            list.Add(term);
        }

        // 返回反正切展式，例如: pi= + 16 * arctan(1/5) - 4 * arctan(1/239) [Machin] 
        public override string ToString()
        {
            string s = "pi= ";
            foreach (Term term in list) s += term.ToString();
            return s + "[" + name + "]";
        }

        // 将圆周率精确到小数点后 digits 位的值以及所用时间输出到 tw 
        public void Out(TextWriter tw, int digits)
        {
            const int digitsPerGroup = 5;
            const int groupsPerLine = 13;
            const int digitsPerLine = groupsPerLine * digitsPerGroup;
            tw.WriteLine(this);
            TimeSpan elapsed;
            int[] piValue = Compute(digits, out elapsed);
            tw.Write("pi= {0}.", piValue[piValue.Length - 1]);
            int position = 6;
            for (int i = piValue.Length - 2; i >= overDigits; i--, position++)
            {
                tw.Write(piValue[i]);
                if (position % digitsPerLine == 0) tw.WriteLine();
                else if (position % digitsPerGroup == 0) tw.Write(" ");
            }
            if ((position - 1) % digitsPerLine != 0) tw.WriteLine();
            tw.WriteLine("[{0}] DIGITS:{1:N0} ELAPSED:{2}", name, digits, elapsed);
        }

        // 计算圆周率到小数点后 digits 位, 并统计所用时间 elapsed 
        int[] Compute(int digits, out TimeSpan elapsed)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int[] piValue = Compute(digits + overDigits);
            Format(piValue);
            stopWatch.Stop();
            elapsed = stopWatch.Elapsed;
            return piValue;
        }

        // 计算圆周率到小数点后 digits 位, 结果未格式化 
        int[] Compute(int digits)
        {
            int[] pi = new int[digits + 1]; // 圆周率的值, 反序存放，如: 951413 
            int[] tmp = new int[digits + 1]; // 中间计算结果，也是反序存放 
            foreach (Term term in list)
            {
                // arctan(x) = x - x^3/3 + x^5/5 - x^7/7 + x^9/9 - x^11/11 + .. 
                int validDigits = digits;
                int divisor = term.Denominator;
                bool positive = term.Positive;
                Array.Clear(tmp, 0, tmp.Length);
                tmp[digits] = term.Coefficient;
                Divide(true, positive, true, ref validDigits, pi, tmp, divisor);
                positive = !positive;
                divisor *= divisor;
                for (int step = 3; validDigits > 0; positive = !positive, step += 2)
                {
                    Divide(false, true, true, ref validDigits, null, tmp, divisor);
                    Divide(true, positive, false, ref validDigits, pi, tmp, step);
                }
            }
            return pi;
        }

        // 计算 sum += 或 -= (dividend /= 或 / divisor) 
        void Divide(
          bool updateSum,      // 是否更新sum 
          bool positive,       // 系数是否正数 
          bool updateDividend, // 是否更新被除数 
          ref int digits,      // 被除数的有效位数 
          int[] sum,          // 和数 
          int[] dividend,     // 被除数 
          int divisor          // 除数 
          )
        {
            for (int remainder = 0, i = digits; i >= 0; i--)
            {
                int quotient = 10 * remainder + dividend[i];
                remainder = quotient % divisor;
                quotient /= divisor;
                if (updateDividend) dividend[i] = quotient;
                if (!updateSum) continue;
                if (positive) sum[i] += quotient;
                else sum[i] -= quotient;
            }
            if (updateDividend) while (digits > 0 && dividend[digits] == 0) digits--;
        }

        // 将 pi 数据组中的每个元素格式化为个位数 
        void Format(int[] pi)
        {
            for (int quotient = 0, i = 0; i < pi.Length; i++)
            {
                int numerator = pi[i] + quotient;
                quotient = numerator / 10;
                int remainder = numerator % 10;
                if (remainder < 0)
                {
                    remainder += 10;
                    quotient--;
                }
                pi[i] = remainder;
            }
        }
    }

    /// <summary>
    /// 用一组反正切展式表示圆周率 
    /// </summary>
    public sealed class Pi
    {
        List<ThePi> list = new List<ThePi>();

        /* 从 ini 文件中读入初始配置，其格式如下： 
        # eg. pi = 16 * arctan(1/5) - 4 * arctan(1/239) [Machin] 
        Stormer  + 176 57 + 28 239 - 48 682 + 96 12943 
        Stomer   +  24  8 +  8  57 +  4 239 
        Gauss    +  48 18 + 32  57 - 20 239 
        Machin   +  16  5 -  4 239 
        */
        public Pi(string iniFileName)
        {
            using (StreamReader sr = new StreamReader(iniFileName))
            {
                Regex regex = new Regex(@"\s+");
                for (;;)
                {
                    string s = sr.ReadLine();
                    if (s == null) break;
                    if (s.Length == 0 || s[0] == '#') continue;
                    string[] ss = regex.Split(s);
                    if (ss.Length % 3 != 1) throw new ApplicationException("ini文件栏目数不正确");
                    ThePi thePi = new ThePi(ss[0]);
                    for (int i = 1; i < ss.Length; i += 3)
                        thePi.Add(new ThePi.Term(ss[i][0] == '+', int.Parse(ss[i + 1]), int.Parse(ss[i + 2])));
                    list.Add(thePi);
                }
            }
        }

        public Pi()
        {
            List<string> formulaList = new List<string>()
            {
                "#eg. pi = 16 * arctan(1/5) - 4 * arctan(1/239) [Machin]",
                "Stormer + 176 57 + 28 239 - 48 682 + 96 12943",
                "Stomer + 24  8 + 8  57 + 4 239",
                "Gauss + 48 18 + 32  57 - 20 239",
                "Machin + 16  5 - 4 239"
            };
            Regex regex = new Regex(@"\s+");
            foreach (string s in formulaList)
            {
                if (s == null) break;
                if (s.Length == 0 || s[0] == '#') continue;
                string[] ss = regex.Split(s);
                if (ss.Length % 3 != 1) throw new ApplicationException("ini文件栏目数不正确");
                ThePi thePi = new ThePi(ss[0]);
                for (int i = 1; i < ss.Length; i += 3)
                    thePi.Add(new ThePi.Term(ss[i][0] == '+', int.Parse(ss[i + 1]), int.Parse(ss[i + 2])));
                list.Add(thePi);
            }

        }


        // 输出全部的反正切展式 
        public void Out(TextWriter tw)
        {
            int i = 0;
            foreach (ThePi thePi in list) tw.WriteLine("{0,2}: {1}", ++i, thePi);
        }

        // 使用 formula 展式计算圆周率精确到小数点后 digits 位，并将其值以及所用时间输出到 tw 
        public void Out(TextWriter tw, int formula, int digits)
        {
            list[formula].Out(tw, digits);
        }
    }

}