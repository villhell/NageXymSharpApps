using Newtonsoft.Json;

namespace NageXymSharpApps.Shared.Models
{
    /// <summary>
    /// Get mosaic information Response Model
    /// 
    /// https://symbol.github.io/symbol-openapi/v1.0.4/#tag/Mosaic-routes/operation/getMosaic
    /// </summary>
    public class MosaicResponse
    {
        [JsonProperty("mosaic")]
        public MosaicInfo MosaicInfo { get; set; }
    }
}
