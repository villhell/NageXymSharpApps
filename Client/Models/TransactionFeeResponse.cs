namespace NageXymSharpApps.Client.Models
{
    /// <summary>
    /// Average = Math.Truncate(minFeeMultiplier:10 + averageFeeMultiplier:113 * 0.65)
    /// </summary>
    public class TransactionFeeResponse
    {
        public int AverageFeeMultiplier { get; set; }
        public int MedianFeeMultiplier { get; set; }
        public int HighestFeeMultiplier { get; set; }
        public int LowestFeeMultiplier { get; set; }
        public int MinFeeMultiplier { get; set; }
    }
}