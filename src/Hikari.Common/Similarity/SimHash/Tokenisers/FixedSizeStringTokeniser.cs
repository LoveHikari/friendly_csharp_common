// Created by: Aref Karimi
// For : SimHash
// Project: SimHashBusiness
// On:  02/02/2012
// 

using System;
using System.Collections.Generic;
using System.Linq;
using Hikari.Common.Similarity.SimHash.Interfaces;

namespace Hikari.Common.Similarity.SimHash.Tokenisers
{
    #region

    

    #endregion
    /// <summary>
    /// 
    /// </summary>
    public class FixedSizeStringTokeniser : ITokeniser
    {
        #region Constants and Fields

        private readonly ushort tokensize = 5;

        #endregion

        #region Constructors and Destructors

        public FixedSizeStringTokeniser(ushort tokenSize)
        {
            if (tokenSize < 2 || tokenSize > 127)
            {
                throw new ArgumentException("Token size must be between 2 and 127");
            }
            this.tokensize = tokenSize;
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<string> Tokenise(string input)
        {
            var chunks = new List<string>();
            int offset = 0;
            while (offset < input.Length)
            {
                chunks.Add(new string(input.Skip(offset).Take(this.tokensize).ToArray()));
                offset += this.tokensize;
            }
            return chunks;
        }

        #endregion
    }
}