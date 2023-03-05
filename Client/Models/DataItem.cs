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
        /// MosaicId
        /// </summary>
        public string MosaicId { get; set; }
        /// <summary>
        /// Divisibility
        /// </summary>
        public int Divisibility { get; set; }
        /// <summary>
        /// Amount
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// ValidAddress
        /// </summary>
        public bool ValidAddress { get; set; }
        /// <summary>
        /// ValidMosaicId
        /// </summary>
        public bool ValidMosaicId { get; set; }
    }
}
