namespace NageXymSharpApps.Shared.Models
{
    /// <summary>
    /// Get mosaic information Response Model
    /// 
    /// https://symbol.github.io/symbol-openapi/v1.0.4/#tag/Mosaic-routes/operation/getMosaic
    /// </summary>
    public class MosaicInfo
    {
        public int Version { get; set; }
        public string Id { get; set; }
        public string Supply { get; set; }
        public string StartHeight { get; set; }
        public string OwnerAddress { get; set; }
        public int Revision { get; set; }
        public int Flags { get; set; }
        public int Divisibility { get; set; }
        public string Duration { get; set; }
    }
}
