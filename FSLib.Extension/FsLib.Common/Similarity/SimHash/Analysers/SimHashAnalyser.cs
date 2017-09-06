using System.Collections.Generic;
using System.Similarity.SimHash.Interfaces;
using System.Similarity.SimHash.Tokenisers;

// Created by: Aref Karimi
// htp://AspGuy.wordpress.com
// For : SimHash
// Project: SimHashBusiness
// On:  02/02/2012
// 
/******************************************************************************************************************
 * 
 * 
 * 标  题：SimHash计算文本相似度类(版本：Version1.0.0)
 * 作  者：YuXiaoWei
 * 日  期：2016/10/20
 * 修  改：
 * 参  考： https://simhash.codeplex.com/
 * 说  明： SimHash事先把每篇文章降维到一个局部哈希数字，计算相似度的时候只需要计算对应的hash值，因此速度比较快，但是测试发现对于短文不误判率比较高，因此建议大于500字以上的使用此算法。
 * 备  注： 暂无...
 * 调用示列：IAnalyser analyser = new SimHashAnalyser();
 *          var likeness = analyser.GetLikenessValue(Needle, HayStack);
 * 
 * 
 * ***************************************************************************************************************/
namespace System.Similarity.SimHash.Analysers
{
    /// <summary>
    /// SimHash计算文本相似度类
    /// </summary>
    public class SimHashAnalyser : IAnalyser
    {
        #region 常量和字段

        private const int HashSize = 32;

        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// 获得相似度
        /// </summary>
        /// <param name="needle">待比较文本</param>
        /// <param name="haystack">模板文本</param>
        /// <returns>相似度</returns>
        public float GetLikenessValue(string needle, string haystack)
        {
            var needleSimHash = this.DoCalculateSimHash(needle);
            var hayStackSimHash = this.DoCalculateSimHash(haystack);
            return (HashSize - GetHammingDistance(needleSimHash, hayStackSimHash)) / (float)HashSize;
        }

        #endregion

        #region Methods
        /// <summary>
        /// 转hash令牌
        /// </summary>
        /// <param name="tokens">令牌</param>
        /// <returns>hash令牌</returns>
        private static IEnumerable<int> DoHashTokens(IEnumerable<string> tokens)
        {
            var hashedTokens = new List<int>();
            foreach (string token in tokens)
            {
                hashedTokens.Add(token.GetHashCode());
            }
            return hashedTokens;
        }

        private static int GetHammingDistance(int firstValue, int secondValue)
        {
            var hammingBits = firstValue ^ secondValue;
            var hammingValue = 0;
            for (int i = 0; i < 32; i++)
            {
                if (IsBitSet(hammingBits, i))
                {
                    hammingValue += 1;
                }
            }
            return hammingValue;
        }

        private static bool IsBitSet(int b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }

        private int DoCalculateSimHash(string input)
        {
            ITokeniser tokeniser = new OverlappingStringTokeniser(4, 3);
            var hashedtokens = DoHashTokens(tokeniser.Tokenise(input));
            var vector = new int[HashSize];
            for (var i = 0; i < HashSize; i++)
            {
                vector[i] = 0;
            }

            foreach (var value in hashedtokens)
            {
                for (var j = 0; j < HashSize; j++)
                {
                    if (IsBitSet(value, j))
                    {
                        vector[j] += 1;
                    }
                    else
                    {
                        vector[j] -= 1;
                    }
                }
            }

            var fingerprint = 0;
            for (var i = 0; i < HashSize; i++)
            {
                if (vector[i] > 0)
                {
                    fingerprint += 1 << i;
                }
            }
            return fingerprint;
        }

        #endregion
    }
}