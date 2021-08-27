using System;
using System.Collections.Generic;
using System.Security.Cryptography;

/******************************************************************************************************************
 * 
 * 
 * 标  题：随机数帮助类(版本：Version1.0.0)
 * 作　者：YuXiaoWei
 * 日　期：2016/10/18
 * 修　改：
 * 参　考：
 * 说　明： 暂无...
 * 备　注： 暂无...
 * 
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common
{
    /// <summary>
    /// 随机数帮助类
    /// </summary>
    public class RandomHelper
    {
        #region 生成随机数
        /// <summary>
        /// 返回非负随机数
        /// </summary>
        /// <returns></returns>
        public int NextNumber()
        {
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            return ran.Next();
        }
        /// <summary>
        /// 返回一个指定范围内的随机数
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public int NextNumber(int minValue, int maxValue)
        {
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            return ran.Next(minValue, maxValue);
        }
        /// <summary>
        /// 返回一个小于所指定最大值的非负随机数
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public int NextNumber(int maxValue)
        {
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            return ran.Next(maxValue);
        }

        /// <summary> 
        /// 产生一个非负数的乱数 
        /// </summary> 
        public int Next()
        {
            var rngp = RandomNumberGenerator.Create();

            byte[] rb = new byte[4];
            rngp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            if (value < 0) value = -value;
            return value;
        }
        /// <summary> 
        /// 产生一个非负数且最大值在 max 以下的乱数 
        /// </summary> 
        /// <param name="max">最大值</param> 
        public int Next(int max)
        {
            var rngp = RandomNumberGenerator.Create();
            byte[] rb = new byte[4];
            rngp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            value = value % max;
            if (value < 0) value = -value;
            return value;
        }
        /// <summary> 
        /// 产生一个非负数且最小值在 min 以上最大值在 max 以下的乱数 
        /// </summary> 
        /// <param name="min">最小值</param> 
        /// <param name="max">最大值</param> 
        public int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }
        #endregion

    }
    /// <summary>
    /// 随机数扩展类
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="ran"></param>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string RandomNumber(this Random ran, int length, bool sleep = false)
        {
            if (sleep) System.Threading.Thread.Sleep(3);
            string result = "";
            RandomHelper randomHelper = new RandomHelper();
            for (int i = 0; i < length; i++)
            {
                result += randomHelper.Next(10).ToString();
            }
            return result;
        }

        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="ran"></param>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Str(this Random ran, int length, bool sleep = false)
        {
            if (sleep) System.Threading.Thread.Sleep(3);
            char[] pattern = new char[]
            {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
                'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };
            string result = "";
            int n = pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(0, n);
                result += pattern[rnd];
            }
            return result;
        }

        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="ran"></param>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Str_char(this Random ran, int length, bool sleep = false)
        {
            if (sleep) System.Threading.Thread.Sleep(3);
            char[] pattern = new char[]
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z'
            };
            string result = "";
            int n = pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(0, n);
                result += pattern[rnd];
            }
            return result;
        }
        /// <summary>
        /// 生成一组不重复的int数组
        /// </summary>
        /// <param name="ran"></param>
        /// <param name="length">数组长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static int[] IntArray(this Random ran, int length, bool sleep = false)
        {
            if (sleep) System.Threading.Thread.Sleep(3);
            List<int> list = new List<int>();
            for (int i = 0; i < length; i++)
            {
                int n = ran.Next();
                if (!list.Contains(n))
                {
                    list.Add(n);
                }
            }
            return list.ToArray();

        }
        /// <summary>
        /// 生成一组不重复的int数组
        /// </summary>
        /// <param name="ran"></param>
        /// <param name="max">数组中数字的上限</param>
        /// <param name="length">数组长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static int[] IntArray(this Random ran, int max, int length, bool sleep = false)
        {
            if (sleep) System.Threading.Thread.Sleep(3);
            List<int> list = new List<int>();
            for (int i = 0; i < length; i++)
            {
                int n = ran.Next(max);
                if (!list.Contains(n))
                {
                    list.Add(n);
                }
            }
            return list.ToArray();

        }
        /// <summary>
        /// 生成一组不重复的int数组
        /// </summary>
        /// <param name="ran"></param>
        /// <param name="min">数组中数字的下限</param>
        /// <param name="max">数组中数字的上限</param>
        /// <param name="length">数组长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static int[] IntArray(this Random ran, int min, int max, int length, bool sleep = false)
        {
            if (sleep) System.Threading.Thread.Sleep(3);
            List<int> list = new List<int>();
            for (int i = 0; i < length; i++)
            {
                int n = ran.Next(min, max);
                if (!list.Contains(n))
                {
                    list.Add(n);
                }
            }
            return list.ToArray();

        }
    }
}