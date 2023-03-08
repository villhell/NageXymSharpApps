namespace NageXymSharpApps.Client.Models
{
    public class Enums
    {
        public enum SymbolNetworkType
        {
            Mainnet = 104,
            Testnet = 152
        }

        public enum CurrencyMosaic : ulong
        {
            /// <summary>
            /// MainnetのxymのモザイクID
            /// </summary>
            Mainnet = 0x6BED913FA20223F8,
            /// <summary>
            /// TestnetのxymのモザイクID
            /// </summary>
            Testnet = 0x72C0212E67A08BCE
        }
    }
}
