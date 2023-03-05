using CatSdk.Symbol;
using CatSdk.Utils;
using NageXymSharpApps.Shared.Models;
using Newtonsoft.Json;
using System.Text;

namespace NageXymSharpApps.Client.Modules
{
    internal class Utils
    {
        #region 文字列からNamespaceIdに変換する。
        /// <summary>
        /// 文字列からNamespaceIdに変換する。
        /// Namespaceは3階層まで存在し、"."で区切って表現される
        /// ex) Level0.Level1.Level2
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        internal static string StringToNamespaceId(string namespaceString)
        {
            if (!string.IsNullOrEmpty(namespaceString))
            {
                // symbol.xymのような場合は最下層のNamespaceIdを使う
                string[] namespaces = namespaceString.Split('.');
                ulong ulongNamespaceId = 0;

                // 1階層
                if(namespaces.Length == 1)
                {
                    var b0 = Converter.Utf8ToBytes(namespaces[0]);
                    var ulongNamespaceId_b0 = IdGenerator.GenerateNamespaceId(b0);
                    ulongNamespaceId = ulongNamespaceId_b0;
                }
                // 2階層
                else if(namespaces.Length == 2)
                {
                    var b0 = Converter.Utf8ToBytes(namespaces[0]);
                    var b1 = Converter.Utf8ToBytes(namespaces[1]);
                    var ulongNamespaceId_b0 = IdGenerator.GenerateNamespaceId(b0);
                    var ulongNamespaceId_b1 = IdGenerator.GenerateNamespaceId(b1, ulongNamespaceId_b0);
                    ulongNamespaceId = ulongNamespaceId_b1;
                }
                // 3階層
                else
                {
                    var b0 = Converter.Utf8ToBytes(namespaces[0]);
                    var b1 = Converter.Utf8ToBytes(namespaces[1]);
                    var b2 = Converter.Utf8ToBytes(namespaces[2]);
                    var ulongNamespaceId_b0 = IdGenerator.GenerateNamespaceId(b0);
                    var ulongNamespaceId_b1 = IdGenerator.GenerateNamespaceId(b1, ulongNamespaceId_b0);
                    var ulongNamespaceId_b2 = IdGenerator.GenerateNamespaceId(b2, ulongNamespaceId_b1);
                    ulongNamespaceId = ulongNamespaceId_b2;
                }
                return ulongNamespaceId.ToString("X16");
            }

            // 変換されなかった場合はそのまま返却
            return namespaceString;
        }
        #endregion

        #region 末尾のスラッシュを削除する
        /// <summary>
        /// 末尾のスラッシュを削除する
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        internal static string TrimingLastSlash(string s)
        {
            return s.Trim('/');
        }
        #endregion

        #region 文字数が指定数と一致するか
        /// <summary>
        /// 文字数が指定数と一致するか
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static bool CheckWordCount(string str, int count)
        {
            if (!string.IsNullOrEmpty(str))
            {
                // 文字数が一致
                if (str.Length == count)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion



        #region アドレス制限情報があるか
        /// <summary>
        /// アドレス情報を取得
        /// </summary>
        /// <param name="address"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        internal async static Task<bool> IsRestrictionAccountAsync(string address, HttpClient client, string node)
        {
            bool ret = false;

            try
            {
                var restrictionsAccountResult = await client.GetAsync(string.Format("{0}/restrictions/account?address={1}", node, address));
                var content = await restrictionsAccountResult.Content.ReadAsStringAsync();
                var restrictionsAccount = JsonConvert.DeserializeObject<RestrictionsAccountResponse>(content);

                // 受信制限なし
                if (restrictionsAccount!.Data.Count == 0) ret = false;
                else ret = true;
            }
            catch (Exception)
            {
                // エラーの場合でも制限なしとする
                return false;
            }

            return ret;
        }
        #endregion

        #region ノードのネットワークとアドレスのネットワークが一致しているか
        /// <summary>
        /// ノードのネットワークとアドレスのネットワークが一致しているか
        /// </summary>
        /// <returns></returns>
        internal async static Task<bool> EqualsNetwork(string nodeUrl, string txtNetwork, HttpClient client)
        {
            try
            {
                var networkInfo = await GetNetworkInfo(nodeUrl, client);

                if (string.Equals(networkInfo!.Name, txtNetwork))
                {
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        #endregion

        #region ノードのネットワーク情報を取得
        /// <summary>
        /// ノードのネットワーク情報を取得
        /// </summary>
        /// <returns></returns>
        internal async static Task<NetworkResponse> GetNetworkInfo(string nodeUrl, HttpClient client)
        {
            NetworkResponse ret = null;
            try
            {
                var networkResult = await client.GetAsync(nodeUrl + "/network");
                var content = await networkResult.Content.ReadAsStringAsync();
                var networkResponse = JsonConvert.DeserializeObject<NetworkResponse>(content);

                if (!string.IsNullOrEmpty(networkResponse!.Name))
                {
                    return networkResponse;
                }
            }
            catch (Exception)
            {
                return ret;
            }
            return ret;
        }
        #endregion

        #region 手数料情報を取得
        /// <summary>
        /// 手数料情報を取得
        /// </summary>
        /// <param name="nodeUrl"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        internal async static Task<TransactionFeeResponse> GetTransactionFee(string nodeUrl, HttpClient client)
        {
            TransactionFeeResponse ret = null;
            try
            {
                var feeResult = await client.GetAsync(nodeUrl + "/network/fees/transaction");
                var content = await feeResult.Content.ReadAsStringAsync();
                ret = JsonConvert.DeserializeObject<TransactionFeeResponse>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ret;
        }
        #endregion

        #region 手数料を計算する

        internal static int CalcFee(int minFeeMultiplier, int averageFeeMultiplier)
        {
            return (int)Math.Round(minFeeMultiplier + averageFeeMultiplier * 0.65, MidpointRounding.AwayFromZero);
        }
        #endregion

        #region トランザクションをアナウンスする
        /// <summary>
        /// トランザクションをアナウンスする
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="nodeUrl"></param>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        internal async static Task<TransactionAnnounceResponse> AnnounceTransactionAsync(string payload, string nodeUrl, HttpClient httpClient)
        {
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(nodeUrl + "/transactions", content);
            var responseDetailsJson = await response.Content.ReadAsStringAsync();
            var announceResponse = JsonConvert.DeserializeObject<TransactionAnnounceResponse>(responseDetailsJson);
            return announceResponse;
        }
        #endregion
    }
}