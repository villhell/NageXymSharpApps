namespace NageXymSharpApps.Client.Models
{
    public class SssExtention
    {
        public bool isSet { get; set; }
        public bool signedFrag { get; set; }
        public object signedTx { get; set; }
        public object encryptMessage { get; set; }
        public string activePublicKey { get; set; }
        public string activeAddress { get; set; }
        public string activeName { get; set; }
        public int activeNetworkType { get; set; }
    }
}
