using NageXymSharpApps.Client.Models;
using NageXymSharpApps.Shared.Models;

namespace nagexymsharpweb.Models
{
    /// <summary>
    /// テーブルデータアイテム
    /// </summary>
    public class DataItem
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Check
        /// </summary>
        public string Check { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Twitter
        /// </summary>
        public string Twitter { get; set; }
        /// <summary>
        /// NamespaceAddress
        /// </summary>
        public string AddressNamespace { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// NamespaceMosaic
        /// </summary>
        public string MosaicNamespace { get; set; }
        /// <summary>
        /// MosaicNamespaceInfo
        /// </summary>
        public MosaicNamespaceInfo MosaicNamespaceInfo { get; set; }
        /// <summary>
        /// Amount
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Valid
        /// </summary>
        public bool Valid { get; set; }
    }
}
