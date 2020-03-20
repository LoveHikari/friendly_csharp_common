// Created by: Aref Karimi
// For : SimHash
// Project: SimHashBusiness
// On:  02/02/2012
// 

using System.Collections.Generic;

namespace System.Similarity.SimHash.Interfaces
{
    #region

    

    #endregion
    /// <summary>
    /// 
    /// </summary>
    public interface ITokeniser
    {
        #region Public Methods and Operators

        IEnumerable<string> Tokenise(string input);

        #endregion
    }
}