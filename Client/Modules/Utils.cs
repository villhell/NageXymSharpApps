using CatSdk.Symbol;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using NageXymSharpApps.Client.Models;

namespace NageXymSharpApps.Client.Modules
{
    internal class Utils
    {
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

        /// <summary>
        /// 末尾のスラッシュを削除する
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        internal static string TrimingLastSlash(string s)
        {
            return s.Trim('/');
        }

        /// <summary>
        /// アドレスかどうか
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        internal static bool CheckAddress(string address)
        {
            if (!string.IsNullOrEmpty(address))
            {
                // 39文字ならアドレスの可能性がある
                if (address.Length == 39)
                {
                    return true;
                }
            }
            return false;
        }

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
                if (CheckAddress(address))
                {
                    var accountResult = await client.GetAsync(string.Format("{0}/accounts/{1}",node , address));
                    var content = await accountResult.Content.ReadAsStringAsync();
                    ret = JsonConvert.DeserializeObject<AccountResponse>(content);
                    
                    if (ret!.Account == null) return null;
                    else return ret;
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
    }
}
