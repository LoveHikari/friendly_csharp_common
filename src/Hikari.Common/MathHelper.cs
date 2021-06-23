using System;

namespace Hikari.Common
{
    /// <summary>
    /// <see cref="Math"/> 扩展类
    /// </summary>
    public class MathHelper
    {
        #region 求最大公约数
        /// <summary>
        /// 辗转相除法求最大公约数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int GetGreatestCommonDivisor3(int a, int b)
        {
            int result = 1;
            if (a > b)
                result = Gcd(a, b);
            else
                result = Gcd(b, a);
            return result;
        }
        private static int Gcd(int a, int b)
        {
            if (a % b == 0)
                return b;
            else
                return Gcd(b, a % b);
        }
        /// <summary>
        /// 更相减损术求最大公约数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int GetGreatestCommonDivisor2(int a, int b)
        {
            if (a == b)
                return a;
            if (a < b)
                return GetGreatestCommonDivisor2(b - a, a);
            else
                return GetGreatestCommonDivisor2(a - b, b);
        }
        /// <summary>
        /// 更相减损术与移位结合求最大公约数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <remarks>参考：http://blog.jobbole.com/106315/?utm_source=blog.jobbole.com&amp;utm_medium=relatedPosts </remarks>
        public static int GetGreatestCommonDivisor(int a, int b)
        {
            if (a >> 1 == 0 || b >> 1 == 0) return 1;
            if (a == b)
                return a;
            if (a < b)
                return GetGreatestCommonDivisor(b, a); //保证参数a永远大于参数b，为减少代码量
            else
            {
                if ((a & 1) == 0 && (b & 1) == 0)
                    return GetGreatestCommonDivisor(a >> 1, b >> 1) << 1;
                else if ((a & 1) == 0 && (b & 1) != 0)
                    return GetGreatestCommonDivisor(a >> 1, b);
                else if ((a & 1) != 0 && (b & 1) == 0)
                    return GetGreatestCommonDivisor(a, b >> 1);
                else
                    return GetGreatestCommonDivisor(b, a - b);

            }
        }


        #endregion
    }
}