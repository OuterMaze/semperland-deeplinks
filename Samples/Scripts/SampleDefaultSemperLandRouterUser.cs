using System;
using UnityEngine;

namespace OuterMaze.Unity.SemperLand.Intents
{
    namespace Samples
    {
        using AlephVault.Unity.DeepLinks.Types;
        using Types.Routing;
        
        public class SampleDefaultSemperLandRouterUser : MonoBehaviour
        {
            private DefaultSemperlandRouter semperlandRouter;

            private const string contractAddress = "12AE66CDc592e10B60f9097a7b0D3C59fce29876";
            private const string targetAddress = "6827b8f6cc60497d9bf5210d602C0EcaFDF7C405";
            private const string brand = "Cd2a3d9f938e13Cd947eC05ABC7fe734df8DD826";
            private const string token1 = "8000000000000000000000000000000000000000000000000000000000001234";
            private const string token2 = "1234";
            private const string token3 = "3";
            private const string wmatic = "8000000000000000000000000000000000000000000000000000000000000000";
            private const string beat = "8000000000000000000000000000000000000000000000000000000000000001";

            private void Awake()
            {
                semperlandRouter = new DefaultSemperlandRouter();
                semperlandRouter.OnEthSend += PrintIntent;
                semperlandRouter.OnERC20View += PrintIntent;
                semperlandRouter.OnERC20Send += PrintIntent;
                semperlandRouter.OnERC721View += PrintIntent;
                semperlandRouter.OnERC721ViewToken += PrintIntent;
                semperlandRouter.OnERC721Send += PrintIntent;
                semperlandRouter.OnERC777View += PrintIntent;
                semperlandRouter.OnERC777Send += PrintIntent;
                semperlandRouter.OnERC1155ViewToken += PrintIntent;
                semperlandRouter.OnERC1155Send += PrintIntent;
                semperlandRouter.OnSmprViewTokenIntent += PrintIntent;
                semperlandRouter.OnSmprSendIntent += PrintIntent;
            }

            private void Start()
            {
                // This one is ok. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"eth:/send?to={targetAddress}&amount=3"));
                // This one is ok. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc20://{contractAddress}"));
                // This one is ok. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc20://{contractAddress}/send?to={targetAddress}&amount=3fffffff"));
                // This one is wrong and should NOT be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc20://{contractAddress}/send?to={targetAddress}&amount=2.5"));
                // This one is ok. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc20://{contractAddress}/send?to={targetAddress}&amount=2.5eth"));
                // This one is ok. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc777://{contractAddress}"));
                // This one is ok. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc777://{contractAddress}/send?to={targetAddress}&amount=3fffffff"));
                // This one is wrong and should NOT be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc777://{contractAddress}/send?to={targetAddress}&amount=2.5"));
                // This one is ok. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc777://{contractAddress}/send?to={targetAddress}&amount=2.5eth"));
                // This one is ok. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc721://{contractAddress}"));
                // This one is ok. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc721://{contractAddress}/{token1}"));
                // This one is ok. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc721://{contractAddress}/{token2}"));
                // This one is ok. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc721://{contractAddress}/{token3}"));
                // This one is ok. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc721://{contractAddress}/send?to={targetAddress}&id={token1}"));
                // This one is ok. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc721://{contractAddress}/send?to={targetAddress}&id={token2}"));
                // This one is ok. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc721://{contractAddress}/send?to={targetAddress}&id={token3}"));
                // This one is wrong and should NOT be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc721://{contractAddress}/send?to={targetAddress}&id=2.5"));
                // This one is wrong and should NOT be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc721://{contractAddress}/send?to={targetAddress}&amount=2"));
                // This one is wrong and should NOT be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc721://{contractAddress}/send?to={targetAddress}&amount=2.5"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc1155://{contractAddress}/{token1}"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc1155://{contractAddress}/{token2}"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc1155://{contractAddress}/{token3}"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc1155://{contractAddress}/send?to={targetAddress}&bulk={token1}:1eth"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc1155://{contractAddress}/send?to={targetAddress}&bulk={token2}:100000000000000000"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc1155://{contractAddress}/send?to={targetAddress}&bulk={token3}:1.5eth"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc1155://{contractAddress}/send?to={targetAddress}&bulk={token3}:1.5eth,{token2}:1.5eth"));
                // This one is wrong (aliases are not supported in generic). It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc1155://{contractAddress}/send?to={targetAddress}&bulk=~BEAT:1.5eth,~WMATIC:2eth"));
                // This one is wrong. It should NOT be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc1155://{contractAddress}/send?to={targetAddress}&bulk=~beat:1.5eth,~wmatic:2eth"));
                // This one is wrong. It should NOT be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc1155://{contractAddress}/send?to={targetAddress}&bulk=BEAT:1.5eth,WMATIC:2eth"));
                // This one is wrong. It should NOT be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"erc1155://{contractAddress}/send?to={targetAddress}&bulk={token3}:1.5ethz"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"smpr://economy/{token1}"));
                // This one is wrong. It should NOT be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"smpr://economy/{token2}z"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"smpr://economy/{token2}"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"smpr://economy/{token3}"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"smpr://economy/{brand}/{token2}"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"smpr://economy/{brand}/{token3}"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"smpr://economy/send?to={targetAddress}&bulk={token1}:1eth"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"smpr://economy/send?to={targetAddress}&bulk={token2}:100000000000000000"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"smpr://economy/send?to={targetAddress}&bulk={token3}:1.5eth"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"smpr://economy/send?to={targetAddress}&bulk={token3}:1.5eth,{token2}:1.5eth"));
                // This one is OK. It should be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"smpr://economy/send?to={targetAddress}&bulk=~BEAT:1.5eth,~WMATIC:2eth"));
                // This one is wrong. It should NOT be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"smpr://economy/send?to={targetAddress}&bulk=~beat:1.5eth,~wmatic:2eth"));
                // This one is wrong. It should NOT be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"smpr://economy/send?to={targetAddress}&bulk=BEAT:1.5eth,WMATIC:2eth"));
                // This one is wrong. It should NOT be printed.
                semperlandRouter.ProcessDeepLink(new Uri($"smpr://economy/send?to={targetAddress}&bulk={token3}:1.5ethz"));
            }

            private void PrintIntent<IntentType>(IntentType intent) where IntentType : DeepLinkModel
            {
                Debug.Log($"Exporting {typeof(IntentType).FullName} intent: {semperlandRouter.ExportDeepLink(intent)}");
            }
        }
    }
}