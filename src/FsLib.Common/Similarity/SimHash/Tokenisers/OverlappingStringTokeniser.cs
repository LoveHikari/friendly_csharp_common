// Created by: Aref Karimi
// For : SimHash
// Project: SimHashBusiness
// On:  02/02/2012
// 

using System.Collections.Generic;
using System.Similarity.SimHash.Interfaces;

namespace System.Similarity.SimHash.Tokenisers
{
    #region

    

    #endregion
    /// <summary>
    /// 
    /// </summary>
    public class OverlappingStringTokeniser : ITokeniser
    {
        #region Constants and Fields

        private readonly ushort chunkSize = 4;

        private readonly ushort overlapSize = 3;

        #endregion

        #region Constructors and Destructors

        public OverlappingStringTokeniser(ushort chunkSize, ushort overlapSize)
        {
            if (chunkSize <= overlapSize)
            {
                throw new ArgumentException("Chunck size must be greater than overlap size.");
            }
            this.overlapSize = overlapSize;
            this.chunkSize = chunkSize;
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<string> Tokenise(string input)
        {
            var result = new List<string>();
            int position = 0;
            while (position < input.Length - this.chunkSize)
            {
                result.Add(input.Substring(position, this.chunkSize));
                position += this.chunkSize - this.overlapSize;
            }
            return result;
        }

        #endregion
    }
}