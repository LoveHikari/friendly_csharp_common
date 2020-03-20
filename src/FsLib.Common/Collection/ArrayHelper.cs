namespace System.Collection
{
    /// <summary>
    /// 数组 帮助类
    /// </summary>
    public class ArrayHelper
    {
        /// <summary>
        /// 数组横纵颠倒
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="t">需要颠倒的数组</param>
        /// <returns>颠倒后的数组</returns>
        public static T[][] ArrayInvert<T>(T[][] t)
        {
            int x = t.Length;  //行数
            int y = t[0].Length;  //列数
            for (int i = 1; i < x; i++)
            {
                int max = t[i].Length;
                y = max > y ? max : y;
            }

            T[][] temp = new T[y][];
            for (int i = 0; i < y; i++)
            {
                temp[i] = new T[x];
            }
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    temp[j][i] = t[i][j];
                }
            }
            return temp;
        }
    }
}