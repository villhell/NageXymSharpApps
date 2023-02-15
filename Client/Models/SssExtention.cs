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

    //public enum TransactionType
    //{
    //    /** Reserved entity type. */
    //    RESERVED = 0,
    //    /** Transfer Transaction transaction type. */
    //    TRANSFER = 16724,
    //    /** Register namespace transaction type. */
    //    NAMESPACE_REGISTRATION = 16718,
    //    /** Address alias transaction type */
    //    ADDRESS_ALIAS = 16974,
    //    /** Mosaic alias transaction type */
    //    MOSAIC_ALIAS = 17230,
    //    /** Mosaic definition transaction type. */
    //    MOSAIC_DEFINITION = 16717,
    //    /** Mosaic supply change transaction. */
    //    MOSAIC_SUPPLY_CHANGE = 16973,
    //    /** Mosaic supply revocation transaction. */
    //    MOSAIC_SUPPLY_REVOCATION = 17229,
    //    /** Modify multisig account transaction type. */
    //    MULTISIG_ACCOUNT_MODIFICATION = 16725,
    //    /** Aggregate complete transaction type. */
    //    AGGREGATE_COMPLETE = 16705,
    //    /** Aggregate bonded transaction type */
    //    AGGREGATE_BONDED = 16961,
    //    /** Lock transaction type */
    //    HASH_LOCK = 16712,
    //    /** Secret Lock Transaction type */
    //    SECRET_LOCK = 16722,
    //    /** Secret Proof transaction type */
    //    SECRET_PROOF = 16978,
    //    /** Account restriction address transaction type */
    //    ACCOUNT_ADDRESS_RESTRICTION = 16720,
    //    /** Account restriction mosaic transaction type */
    //    ACCOUNT_MOSAIC_RESTRICTION = 16976,
    //    /** Account restriction operation transaction type */
    //    ACCOUNT_OPERATION_RESTRICTION = 17232,
    //    /** Link account transaction type */
    //    ACCOUNT_KEY_LINK = 16716,
    //    /** Mosaic address restriction type */
    //    MOSAIC_ADDRESS_RESTRICTION = 16977,
    //    /** Mosaic global restriction type */
    //    MOSAIC_GLOBAL_RESTRICTION = 16721,
    //    /** Account metadata transaction */
    //    ACCOUNT_METADATA = 16708,
    //    /** Mosaic metadata transaction */
    //    MOSAIC_METADATA = 16964,
    //    /** Namespace metadata transaction */
    //    NAMESPACE_METADATA = 17220,
    //    /** Link vrf key transaction */
    //    VRF_KEY_LINK = 16963,
    //    /** Link voting key transaction */
    //    VOTING_KEY_LINK = 16707,
    //    /** Link node key transaction */
    //    NODE_KEY_LINK = 16972
    //}

    //public enum NetworkType
    //{
    //    /** Main net network */
    //    MAIN_NET = 104,
    //    /** Test net network */
    //    TEST_NET = 152
    //}
}
