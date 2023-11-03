using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Nethereum.Hex.HexConvertors.Extensions;
using AlephVault.Unity.EVMGames.DeepLinks.Types.Routing;

namespace OuterMaze.SemperLand.Intents
{
    namespace Types
    {
        namespace Routing
        {
            /// <summary>
            ///   A default router (for many default <see cref="Intent" />
            ///   types) which can be extended application-wise.
            /// </summary>
            public class DefaultSemperlandRouter : DefaultEVMRouter
            {
                /// <summary>
                ///   This event processes a Smpr Send intent.
                /// </summary>
                public event Action<SmprSendDeepLinkModel> OnSmprSendIntent;

                /// <summary>
                ///   This event processes a Smpr View intent.
                /// </summary>
                public event Action<SmprViewTokenDeepLinkModel> OnSmprViewTokenIntent;
                
                /// <summary>
                ///   The supported aliases. They're all system-wide and
                ///   will be assigned a sequential id starting from the
                ///   value <c>(1 &lt;&lt; 255)</c>.
                /// </summary>
                protected virtual string[] SupportedTokenAliases()
                {
                    return new[] {"WMATIC", "BEAT"};
                }

                public DefaultSemperlandRouter()
                {
                    // SMPR
                    AddParsingRule("smpr/token")
                        .MatchingScheme("smpr")
                        .MatchingAuthority("economy")
                        .MatchingPath(new Regex(@"^/([a-fA-F0-9]{1,64})$"))
                        .BuildingDeepLinkModelAs(result =>
                        {
                            IsBigInt(result.PathMatches[1][0], out BigInteger tokenId);
                            return new SmprViewTokenDeepLinkModel(result.Uri, tokenId);
                        });
                    AddParsingRule("smpr/token2")
                        .MatchingScheme("smpr")
                        .MatchingAuthority("economy")
                        .MatchingPath(new Regex(@"^/([a-fA-F0-9]{40})/([a-f0-9]{1,16})$"))
                        .BuildingDeepLinkModelAs(result =>
                        {
                            IsBigInt(result.PathMatches[1][0], out BigInteger brandId);
                            IsBigInt(result.PathMatches[2][0], out BigInteger tokenId);
                            return new SmprViewTokenDeepLinkModel(result.Uri, brandId << 64 | tokenId | new BigInteger(1) << 255);
                        });
                    OnDeepLink<SmprViewTokenDeepLinkModel>(intent => OnSmprViewTokenIntent?.Invoke(intent));
                    AddExportRule<SmprViewTokenDeepLinkModel>(intent =>
                    {
                        return $"smpr://economy/{intent.Id.ToHex(false).Substring(2)}";
                    });

                    Dictionary<string, BigInteger> aliases = new Dictionary<string, BigInteger>();
                    Dictionary<BigInteger, string> aliasesInverse = new Dictionary<BigInteger, string>();
                    string[] supportedTokenAliases = SupportedTokenAliases();
                    BigInteger startToken = new BigInteger(1) << 255;
                    foreach (string supportedTokenAlias in supportedTokenAliases)
                    {
                        aliases[supportedTokenAlias] = startToken;
                        aliasesInverse[startToken] = supportedTokenAlias;
                        startToken++;
                    }
                    AddParsingRule("smpr/send")
                        .MatchingScheme("smpr")
                        .MatchingAuthority("economy")
                        .MatchingPath("/send")
                        .BuildingDeepLinkModelAs(result =>
                        {
                            string to = result.GetQueryStringParameter("to");
                            string bulk = result.GetQueryStringParameter("bulk");
                            if (!IsAddress(to)) return null;
                            if (!IsBulk(bulk, out Tuple<BigInteger, BigInteger>[] items, aliases)) return null;
                            return new SmprSendDeepLinkModel(to, items);
                        });
                    OnDeepLink<SmprSendDeepLinkModel>(intent => OnSmprSendIntent?.Invoke(intent));
                    AddExportRule<SmprSendDeepLinkModel>(intent =>
                    {
                        string bulk = string.Join(
                            ",", intent.Tokens.Select(pair =>
                            {
                                string id = aliasesInverse.TryGetValue(pair.Item1, out string alias) ?
                                    "~" + alias : pair.Item1.ToHex(false).Substring(2);
                                string amount = ExportAmount(pair.Item2);
                                return $"{id}:{amount}";
                            }).ToArray()
                        );
                        return $"smpr://economy/send?to={intent.TargetAddress}?bulk={bulk}";
                    });
                }
            }
        }
    }
}