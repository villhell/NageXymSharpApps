using System;

namespace Api.Server
{
    internal class Utils
    {

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

        #region 手数料を計算する
        /// <summary>
        /// 手数料を計算する
        /// </summary>
        /// <param name="minFeeMultiplier"></param>
        /// <param name="averageFeeMultiplier"></param>
        /// <returns></returns>
        internal static int CalcFee(int minFeeMultiplier, int averageFeeMultiplier)
        {
            return (int)Math.Round(minFeeMultiplier + averageFeeMultiplier * 0.65, MidpointRounding.AwayFromZero);
        }
        #endregion
    }
}