namespace NageXymSharpApps.Client.Models
{
        public class AccountResponse
        {
            public Account Account { get; set; }
            public string Id { get; set; }
        }

        public class Account
        {
            public long Version { get; set; }
            public string Address { get; set; }
            public long AddressHeight { get; set; }
            public string PublicKey { get; set; }
            public long PublicKeyHeight { get; set; }
            public long AccountType { get; set; }
            public SupplementalPublicKeys SupplementalPublicKeys { get; set; }
            public List<object> ActivityBuckets { get; set; }
            public List<Mosaic> Mosaics { get; set; }
            public long Importance { get; set; }
            public long ImportanceHeight { get; set; }
        }

        public class Mosaic
        {
            public string Id { get; set; }
            public long Amount { get; set; }
        }

        public class SupplementalPublicKeys
        {
        }
    

}
