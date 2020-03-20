
/******************************************************************************************************************
 * 
 * 
 * 标  题： 整型帮助类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2017/03/02
 * 修  改：
 * 参  考： 
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：
 *
 * 
 * ***************************************************************************************************************/
namespace System
{
    /// <summary>
    /// 整型帮助类
    /// </summary>
    public class IntegerHelper
    {
        /// <summary>
        /// 是否是奇数
        /// </summary>
        /// <param name="n"></param>
        /// <returns>true：是奇数</returns>
        public static bool IsOdd(int n)
        {
            return (n % 2 == 1);
        }
        /// <summary>
        /// 判断给定的数字是否为素数(质数)
        /// </summary>
        /// <param name="num">true为质数</param>
        /// <returns></returns>
        public static bool IsPrime(int num)
        {
            if (num < 2)
            {
                return false;
            }

            for (int i = 2; i * i <= num; i++)
            {
                if (num % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 将数字的字符串形式转换为等效的32位有符号整数，转换失败返回0
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <returns></returns>
        public static int Parse(string s)
        {
            int result;
            if (int.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
            
        }


    }
}