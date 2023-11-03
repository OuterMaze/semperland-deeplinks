using System;
using System.Numerics;

namespace OuterMaze.SemperLand.Intents
{
    namespace Types 
    {
        using AlephVault.Unity.DeepLinks.Types;
        
        /// <summary>
        ///   Intent to send tokens on the SemperLand (Economy) contract.
        /// </summary>
        public class SmprSendDeepLinkModel : DeepLinkModel
        {
            /// <summary>
            ///   The address to send the tokens to.
            /// </summary>
            public readonly string TargetAddress;

            /// <summary>
            ///   The tokens to send.
            /// </summary>
            public readonly Tuple<BigInteger, BigInteger>[] Tokens;
            
            public SmprSendDeepLinkModel(
                string targetAddress, Tuple<BigInteger, BigInteger>[] tokens
            )
            {
                TargetAddress = targetAddress;
                Tokens = tokens;
            }
        }
    }
}
