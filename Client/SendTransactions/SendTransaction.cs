
using NageXymSharpApps.Shared.CatSdk.CryptoTypes;
using NageXymSharpApps.Shared.CatSdk.Facade;
using NageXymSharpApps.Shared.CatSdk.Symbol;
using NageXymSharpApps.Shared.CatSdk.Symbol.Factory;
using NageXymSharpApps.Shared.CatSdk.Utils;
using System.Text;

namespace NageXymSharpApps.Client.SendTransactions
{
    public class SendTransaction
    {

        #region トランザクションを作成
        /// <summary>
        /// トランザクションを作成
        /// </summary>
        /// <param name="network"></param>
        /// <param name="keyPair"></param>
        /// <param name="address"></param>
        /// <param name="xym"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private EmbeddedTransferTransactionV1 CreateTransaction(Network network, KeyPair keyPair, string address, ulong xym, byte[] message)
        {
            var networkType = string.Equals(network.Name, "mainnet") ? NetworkType.MAINNET : NetworkType.TESTNET;
            ulong mosaicId = (ulong)(string.Equals(network.Name, "mainnet") ? 0x6BED913FA20223F8 : 0x72C0212E67A08BCE);
            return new EmbeddedTransferTransactionV1
            {
                Network = networkType,
                SignerPublicKey = keyPair.PublicKey,
                RecipientAddress = new UnresolvedAddress(Converter.StringToAddress(address)),
                Mosaics = new UnresolvedMosaic[]
                {
                    new()
                    {
                        MosaicId = new UnresolvedMosaicId(mosaicId),
                        Amount = new Amount(xym)
                    }
                },
                Message = message
            };
        }
        #endregion

        #region グリッドのアドレスの内容でアグリゲートコンプリートトランザクションを作成、アナウンスする
        /// <summary>
        /// グリッドのアドレスの内容でアグリゲートコンプリートトランザクションを作成、アナウンスする
        /// 
        /// アグリゲートコンプリートに纏められるトランザクションは100件までなので
        /// 100件毎に纏めてアナウンスする
        /// <param name="network"></param>
        /// </summary>
        private void SendAggregateCompleteTransaction(Network network)
        {
            var facade = new SymbolFacade(network);
            var privateKey = new PrivateKey("PRIVATE_KEY");
            var keyPair = new KeyPair(privateKey);

            var txs = new List<IBaseTransaction>();

            string? address = "ADDRESS";
            string? s = "xym数";
            ulong xym = (ulong)(double.Parse(s) * 1000000);
            string? message = "Message";
            byte[] bytes = Converter.Utf8ToPlainMessage(message);

            txs.Add(CreateTransaction(network, keyPair, address, xym, bytes));

            AnnounceAsync(network, facade, keyPair, txs);
        }
        #endregion

        #region トランザクションをアナウンスする
        /// <summary>
        /// トランザクションをアナウンスする
        /// </summary>
        /// <param name="network"></param>
        /// <param name="facade"></param>
        /// <param name="keyPair"></param>
        /// <param name="txs"></param>
        /// <returns></returns>
        private async Task AnnounceAsync(Network network, SymbolFacade facade, KeyPair keyPair, List<IBaseTransaction> txs)
        {
            var innerTransactions = txs.ToArray();

            var merkleHash = SymbolFacade.HashEmbeddedTransactions(innerTransactions);

            var aggTx = new AggregateCompleteTransactionV2
            {
                Network = string.Equals(network.Name, "mainnet") ? NetworkType.MAINNET : NetworkType.TESTNET,
                Transactions = innerTransactions,
                SignerPublicKey = keyPair.PublicKey,
                Fee = new Amount(1000000),
                TransactionsHash = merkleHash,
                Deadline = new Timestamp(facade.Network.FromDatetime<NetworkTimestamp>(DateTime.UtcNow).AddHours(2).Timestamp),
            };
            //var fee = aggTx.Size * feeMultiplier;
            //aggTx.Fee = new Amount((ulong)fee);
            //aggTx.Fee = new Amount((ulong)1000000);
            var signature = facade.SignTransaction(keyPair, aggTx);
            TransactionsFactory.AttachSignature(aggTx, signature);

            var hash = facade.HashTransaction(aggTx);
            //var bobCosignature = new Cosignature
            //{
            //    Signature = bobKeyPair.Sign(hash.bytes),
            //    SignerPublicKey = bobKeyPair.PublicKey
            //};
            //aggTx.Cosignatures = new[] { bobCosignature };

            var payload = TransactionsFactory.CreatePayload(aggTx);

            string node = Utils.TrimingLastSlash("NODE_URL");
            using var client = new HttpClient();
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = client.PutAsync(node + "/transactions", content).Result;
            var responseDetailsJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseDetailsJson);
        }
        #endregion

        #region アドレス情報を取得できるか
        /// <summary>
        /// アドレス情報を取得できるか
        /// </summary>
        /// <param name="address"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private async Task<string?> ExistsAddressAsync(string address, HttpClient client)
        {
            return null;
        }
        #endregion

        #region ネームスペース情報を取得できるか
        /// <summary>
        /// ネームスペース情報を取得できるか
        /// TODO: 返却するのがAddressになってるのは違和感ある
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="client"></param>
        /// <returns></returns>

        private async Task<string?> ExistsNamespaceAsync(string ns, HttpClient client)
        {
            return null;
        }
        #endregion

        #region 引数が39文字であるか
        /// <summary>
        /// 引数が39文字であるか
        /// 
        /// 39文字でなければアドレスではないはず
        /// 39文字のネームスペースでもExistsNamespaceAsyncで判別できる
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private bool IsAddressLength(string address)
        {
            if (!string.IsNullOrEmpty(address))
            {
                if (address.Length == 39)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
