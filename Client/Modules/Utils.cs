using CatSdk.Symbol;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using NageXymSharpApps.Client.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using AntDesign;
using System.Transactions;

namespace NageXymSharpApps.Client.Modules
{
    internal class Utils
    {
        #region 文字列からNamespaceIdに変換する。
        /// <summary>
        /// 文字列からNamespaceIdに変換する。
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        internal static string StringToNamespaceId(string ns)
        {
            // ネームスペース文字列をバイナリ変換
            var b = Encoding.UTF8.GetBytes(ns);

            // ulongに変換
            var ulong_namespaceid = IdGenerator.GenerateNamespaceId(b);
            
            // バイナリ変換
            var bytes = BitConverter.GetBytes(ulong_namespaceid);

            // リトルエンディアンならビッグエンディアンに変換
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            // バイナリから16進数文字列へ変換
            var namespaceId = Convert.ToHexString(bytes);

            return namespaceId;
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

        #region アドレス情報を取得
        /// <summary>
        /// アドレス情報を取得
        /// </summary>
        /// <param name="address"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        internal async static Task<AccountResponse?> GetAddressAsync(string address, HttpClient client, string node)
        {
            AccountResponse? ret = null;

            try
            {
                // アドレスの文字列かどうか
                if (CheckWordCount(address, 39))
                {
                    var accountResult = await client.GetAsync(string.Format("{0}/accounts/{1}", node, address));
                    var content = await accountResult.Content.ReadAsStringAsync();
                    ret = JsonConvert.DeserializeObject<AccountResponse>(content);

                    if (ret!.Account == null)
                    {
                        return null;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                return null;
            }
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

        internal async static Task<NamespaceResponse?> GetNamespaceAsync(string ns, HttpClient client, string node)
        {
            NamespaceResponse? ret = null;

            try
            {
                var namespaceId = Utils.StringToNamespaceId(ns);
                var namespaceResult = await client.GetAsync(string.Format("{0}/namespaces/{1}", node, namespaceId));
                var content = await namespaceResult.Content.ReadAsStringAsync();
                ret = JsonConvert.DeserializeObject<NamespaceResponse>(content);

                if (ret!.Namespace == null) return null;
                else return ret;
            }
            catch (Exception)
            {
                return null;
            }
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
