using NageXymSharpApps.Shared.Models;

namespace NageXymSharpApps.Client.Validators
{
    /// <summary>
    /// ネームスペースをチェックするためのクラス
    /// </summary>
    public class NamespaceValidator
    {
        /// <summary>
        /// AliasType
        /// </summary>
        private enum AliasType : int
        {
            Mosaic = 1,
            Address = 2
        }

        private Alias _alias;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="alias"></param>
        public NamespaceValidator(Alias alias)
        {
            _alias = alias;
        }

        /// <summary>
        /// ネームスペースに紐づくアドレスが存在するか
        /// </summary>
        /// <returns></returns>
        public bool ExistsAddress()
        {
            return (_alias.Type == (int)AliasType.Address && _alias.Address != null);
        }

        /// <summary>
        /// ネームスペースに紐づくモザイクIDは存在するか
        /// </summary>
        /// <returns></returns>
        public bool ExistsMosaicId()
        {
            return (_alias.Type == (int)AliasType.Mosaic && _alias.MosaicId != null);
        }
    }
}
