using System;
using System.Numerics;

namespace OuterMaze.Unity.SemperLand.Intents
{
    namespace Types 
    {
        using AlephVault.Unity.DeepLinks.Types;
        
        /// <summary>
        ///   Intent to view a token on the SemperLand (Economy) contract.
        ///   The token may be a brand, another type of NFT, or a fungible
        ///   token.
        /// </summary>
        public class SmprViewTokenDeepLinkModel : DeepLinkModel
        {
            /// <summary>
            ///   The token to view.
            /// </summary>
            public readonly BigInteger Id;
            
            /// <summary>
            ///   One way to construct the intent: With the direct ID.
            ///   This one stands for global fungible tokens, brands,
            ///   and other NFTs.
            /// </summary>
            /// <param name="uri">The intent URI</param>
            /// <param name="id">The token id</param>
            public SmprViewTokenDeepLinkModel(Uri uri, BigInteger id)
            {
                Id = id;
            }

            /// <summary>
            ///   Another way to construct the intent: With the brand id
            ///   and the asset index. This one only stands for fungible
            ///   tokens belonging to a brand.
            /// </summary>
            /// <param name="uri"></param>
            /// <param name="brandId"></param>
            /// <param name="index"></param>
            public SmprViewTokenDeepLinkModel(Uri uri, BigInteger brandId, BigInteger index)
            {
                Id = (brandId << 64) | index;
            }
        }
    }
}
