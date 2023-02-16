using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public class SignedTransaction
    {
        /// <summary>
        /// Transaction payload
        /// </summary>
        public string payload { get; set; }
        /// <summary>
        /// Transaction hash
        /// </summary>
        public string hash { get; set; }
        /// <summary>
        /// Transaction signerPublicKey
        /// </summary>
        public string signerPublicKey { get; set; }
        /// <summary>
        /// Transaction type
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// Signer network type
        /// </summary>
        public int networkType { get; set; } 
    }
}
