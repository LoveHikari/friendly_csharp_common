/******************************************************************************************************************
 * 
 * 
 * 标  题：字符串相似度实现类[编辑距离算法(Levenshtein Distance)](版本：Version1.0.0)
 * 作  者：YuXiaoWei
 * 日  期：2016/11/10
 * 修  改：
 * 参  考： http://blog.csdn.net/yangzhongwei1031/article/details/5898330 ,http://www.cnblogs.com/stone_w/archive/2012/08/16/2642679.html
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：// 方式一
            LevenshteinDistance stringcompute1 = new LevenshteinDistance();
            stringcompute1.SpeedyCompute("对比字符一", "对比字符二");    // 计算相似度， 不记录比较时间
            decimal rate = stringcompute1.ComputeResult.Rate;         // 相似度百分之几，完全匹配相似度为1

            // 方式二
            LevenshteinDistance stringcompute2 = new LevenshteinDistance();
            stringcompute2.Compute();                                  // 计算相似度， 记录比较时间
            string usetime = stringcompute2.ComputeResult.UseTime;     // 对比使用时间
 *
 * 
 * ***************************************************************************************************************/

namespace System.Similarity.LevenshteinDistance
{
    /// <summary>
    /// 
    /// </summary>
    public class LevenshteinDistance
    {
        #region 私有变量
        /// <summary>
        /// 字符串1
        /// </summary>
        private char[] _ArrChar1;
        /// <summary>
        /// 字符串2
        /// </summary>
        private char[] _ArrChar2;
        /// <summary>
        /// 统计结果
        /// </summary>
        private Result _Result;
        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTime _BeginTime;
        /// <summary>
        /// 结束时间
        /// </summary>
        private DateTime _EndTime;
        /// <summary>
        /// 计算次数
        /// </summary>
        private int _ComputeTimes;
        /// <summary>
        /// 算法矩阵
        /// </summary>
        private int[,] _Matrix;
        /// <summary>
        /// 矩阵列数
        /// </summary>
        private int _Column;
        /// <summary>
        /// 矩阵行数
        /// </summary>
        private int _Row;
        #endregion
        #region 属性
        public Result ComputeResult
        {
            get { return _Result; }
        }
        #endregion
        #region 构造函数
        public LevenshteinDistance(string str1, string str2)
        {
            this.StringComputeInit(str1, str2);
        }
        public LevenshteinDistance()
        {
        }
        #endregion
        #region 算法实现
        /// <summary>
        /// 初始化算法基本信息
        /// </summary>
        /// <param name="str1">字符串1</param>
        /// <param name="str2">字符串2</param>
        private void StringComputeInit(string str1, string str2)
        {
            _ArrChar1 = str1.ToCharArray();
            _ArrChar2 = str2.ToCharArray();
            _Result = new Result();
            _ComputeTimes = 0;
            _Row = _ArrChar1.Length + 1;
            _Column = _ArrChar2.Length + 1;
            _Matrix = new int[_Row, _Column];
        }
        /// <summary>
        /// 计算相似度
        /// </summary>
        public void Compute()
        {
            //开始时间
            _BeginTime = DateTime.Now;
            //初始化矩阵的第一行和第一列
            this.InitMatrix();
            int intCost = 0;
            for (int i = 1; i < _Row; i++)
            {
                for (int j = 1; j < _Column; j++)
                {
                    if (_ArrChar1[i - 1] == _ArrChar2[j - 1])
                    {
                        intCost = 0;
                    }
                    else
                    {
                        intCost = 1;
                    }
                    //关键步骤，计算当前位置值为左边+1、上面+1、左上角+intCost中的最小值 
                    //循环遍历到最后_Matrix[_Row - 1, _Column - 1]即为两个字符串的距离
                    _Matrix[i, j] = this.Minimum(_Matrix[i - 1, j] + 1, _Matrix[i, j - 1] + 1, _Matrix[i - 1, j - 1] + intCost);
                    _ComputeTimes++;
                }
            }
            //结束时间
            _EndTime = DateTime.Now;
            //相似率 移动次数小于最长的字符串长度的20%算同一题
            int intLength = _Row > _Column ? _Row : _Column;

            _Result.Rate = (1 - (decimal)_Matrix[_Row - 1, _Column - 1] / intLength);
            _Result.UseTime = (_EndTime - _BeginTime).ToString();
            _Result.ComputeTimes = _ComputeTimes.ToString();
            _Result.Difference = _Matrix[_Row - 1, _Column - 1];
        }
        /// <summary>
        /// 两字符串相似度计算方法(编辑距离算法LevenshteinDistance又称EditDistance)
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static string LD(string value1, string value2)
        {
            if (string.IsNullOrEmpty(value1)
                || string.IsNullOrEmpty(value2))
            {
                return "0";
            }


            int n = value1.Length;
            int m = value2.Length;
            int[,] d = new int[n + 1, m + 1];

            int temp = 0;
            char ch1, ch2;
            int i = 0, j = 0;

            for (i = 0; i <= n; i++)
            {
                //初始化第一列
                d[i, 0] = i;
            }
            for (j = 0; j <= m; j++)
            {
                //初始化第一行
                d[0, j] = j;
            }

            for (i = 1; i <= n; i++)
            {
                ch1 = value1[i - 1];
                for (j = 1; j <= m; j++)
                {
                    ch2 = value2[j - 1];
                    if (ch1 == ch2)
                    {
                        temp = 0;
                    }
                    else
                    {
                        temp = 1;
                    }
                    //对比获取最小值
                    int min = 0, first = d[i - 1, j] + 1, second = d[i, j - 1] + 1, third = d[i - 1, j - 1] + temp;
                    min = first < second ? first : second;
                    min = min < third ? min : third;

                    d[i, j] = min;
                }
            }

            //输出百分比 保留两位小数
            int maxLenth = value1.Length > value2.Length ? value1.Length : value2.Length;
            decimal percent = (1 - (decimal)d[n, m] / maxLenth) * 100;
            return $"{percent:F2}%".Replace(".00", "");
        }

        /// <summary>
        /// 计算相似度（不记录比较时间）
        /// </summary>
        public void SpeedyCompute()
        {
            //开始时间
            //_BeginTime = DateTime.Now;
            //初始化矩阵的第一行和第一列
            this.InitMatrix();
            int intCost = 0;
            for (int i = 1; i < _Row; i++)
            {
                for (int j = 1; j < _Column; j++)
                {
                    if (_ArrChar1[i - 1] == _ArrChar2[j - 1])
                    {
                        intCost = 0;
                    }
                    else
                    {
                        intCost = 1;
                    }
                    //关键步骤，计算当前位置值为左边+1、上面+1、左上角+intCost中的最小值 
                    //循环遍历到最后_Matrix[_Row - 1, _Column - 1]即为两个字符串的距离
                    _Matrix[i, j] = this.Minimum(_Matrix[i - 1, j] + 1, _Matrix[i, j - 1] + 1, _Matrix[i - 1, j - 1] + intCost);
                    _ComputeTimes++;
                }
            }
            //结束时间
            //_EndTime = DateTime.Now;
            //相似率 移动次数小于最长的字符串长度的20%算同一题
            int intLength = _Row > _Column ? _Row : _Column;

            _Result.Rate = (1 - (decimal)_Matrix[_Row - 1, _Column - 1] / intLength);
            // _Result.UseTime = (_EndTime - _BeginTime).ToString();
            _Result.ComputeTimes = _ComputeTimes.ToString();
            _Result.Difference = _Matrix[_Row - 1, _Column - 1];
        }
        /// <summary>
        /// 计算相似度
        /// </summary>
        /// <param name="str1">字符串1</param>
        /// <param name="str2">字符串2</param>
        public void Compute(string str1, string str2)
        {
            this.StringComputeInit(str1, str2);
            this.Compute();
        }

        /// <summary>
        /// 计算相似度
        /// </summary>
        /// <param name="str1">字符串1</param>
        /// <param name="str2">字符串2</param>
        public void SpeedyCompute(string str1, string str2)
        {
            this.StringComputeInit(str1, str2);
            this.SpeedyCompute();
        }
        /// <summary>
        /// 初始化矩阵的第一行和第一列
        /// </summary>
        private void InitMatrix()
        {
            for (int i = 0; i < _Column; i++)
            {
                _Matrix[0, i] = i;
            }
            for (int i = 0; i < _Row; i++)
            {
                _Matrix[i, 0] = i;
            }
        }
        /// <summary>
        /// 取三个数中的最小值
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <param name="Third"></param>
        /// <returns></returns>
        private int Minimum(int First, int Second, int Third)
        {
            int intMin = First;
            if (Second < intMin)
            {
                intMin = Second;
            }
            if (Third < intMin)
            {
                intMin = Third;
            }
            return intMin;
        }
        #endregion
    }
    /// <summary>
    /// 计算结果
    /// </summary>
    public struct Result
    {
        /// <summary>
        /// 相似度
        /// </summary>
        public decimal Rate;
        /// <summary>
        /// 对比次数
        /// </summary>
        public string ComputeTimes;
        /// <summary>
        /// 使用时间
        /// </summary>
        public string UseTime;
        /// <summary>
        /// 差异
        /// </summary>
        public int Difference;
    }
}