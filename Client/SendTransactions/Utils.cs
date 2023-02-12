using NageXymSharpApps.Shared.CatSdk.Symbol;
using System.Text;

namespace NageXymSharpApps.Client.SendTransactions
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
    }
}
