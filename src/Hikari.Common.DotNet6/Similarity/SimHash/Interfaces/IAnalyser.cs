// Created by: Aref Karimi
// For : SimHash
// Project: SimHashBusiness
// On:  02/02/2012
// 

namespace Hikari.Common.Similarity.SimHash.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAnalyser
    {
        #region Public Methods and Operators
        /// <summary>
        /// 获得相似度
        /// </summary>
        /// <param name="needle">待比较文本</param>
        /// <param name="haystack">模板文本</param>
        /// <returns>相似度</returns>
        float GetLikenessValue(string needle, string haystack);

        #endregion
    }
}