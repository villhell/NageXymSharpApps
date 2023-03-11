namespace NageXymSharpApps.Shared.Models
{
    public class NetworkPropertiesResponse
    {

        public class Network
        {
            public string Identifier { get; set; }

            public string NemesisSignerPublicKey { get; set; }

            public string NodeEqualityStrategy { get; set; }

            public string GenerationHashSeed { get; set; }

            public string EpochAdjustment { get; set; }
        }

        public class Chain
        {
            public bool EnableVerifiableState { get; set; }
            public bool EnableVerifiableReceipts { get; set; }
            public string CurrencyMosaicId { get; set; }
            public string HarvestingMosaicId { get; set; }
            public string BlockGenerationTargetTime { get; set; }
            public string BlockTimeSmoothingFactor { get; set; }
            public string ImportanceGrouping { get; set; }
            public string ImportanceActivityPercentage { get; set; }
            public string MaxRollbackBlocks { get; set; }
            public string MaxDifficultyBlocks { get; set; }
            public string DefaultDynamicFeeMultiplier { get; set; }
            public string MaxTransactionLifetime { get; set; }
            public string MaxBlockFutureTime { get; set; }
            public string InitialCurrencyAtomicUnits { get; set; }
            public string MaxMosaicAtomicUnits { get; set; }
            public string TotalChainImportance { get; set; }
            public string MinHarvesterBalance { get; set; }
            public string MaxHarvesterBalance { get; set; }
            public string MinVoterBalance { get; set; }
            public string VotingSetGrouping { get; set; }
            public string MaxVotingKeysPerAccount { get; set; }
            public string MinVotingKeyLifetime { get; set; }
            public string MaxVotingKeyLifetime { get; set; }
            public string HarvestBeneficiaryPercentage { get; set; }
            public string HarvestNetworkPercentage { get; set; }
            public string HarvestNetworkFeeSinkAddressV1 { get; set; }
            public string HarvestNetworkFeeSinkAddress { get; set; }
            public string MaxTransactionsPerBlock { get; set; }
        }

        public class AccountLink
        {
            public string Dummy { get; set; }
        }

        public class Aggregate
        {
            public string MaxTransactionsPerAggregate { get; set; }
            public string MaxCosignaturesPerAggregate { get; set; }
            public bool EnableStrictCosignatureCheck { get; set; }
            public bool EnableBondedAggregateSupport { get; set; }
            public string MaxBondedTransactionLifetime { get; set; }
        }

        public class LockHash
        {
            public string LockedFundsPerAggregate { get; set; }
            public string MaxHashLockDuration { get; set; }
        }

        public class LockSecret
        {
            public string MaxSecretLockDuration { get; set; }
            public string MinProofSize { get; set; }
            public string MaxProofSize { get; set; }
        }

        public class Metadata
        {
            public string MaxValueSize { get; set; }
        }

        public class Mosaic
        {
            public string MaxMosaicsPerAccount { get; set; }
            public string MaxMosaicDuration { get; set; }
            public string MaxMosaicDivisibility { get; set; }
            public string MosaicRentalFeeSinkAddressV1 { get; set; }
            public string MosaicRentalFeeSinkAddress { get; set; }
            public string MosaicRentalFee { get; set; }
        }

        public class Multisig
        {
            public string MaxMultisigDepth { get; set; }
            public string MaxCosignatoriesPerAccount { get; set; }
            public string MaxCosignedAccountsPerAccount { get; set; }
        }

        public class Namespace
        {
            public string MaxNameSize { get; set; }
            public string MaxChildNamespaces { get; set; }
            public string MaxNamespaceDepth { get; set; }
            public string MinNamespaceDuration { get; set; }
            public string MaxNamespaceDuration { get; set; }
            public string NamespaceGracePeriodDuration { get; set; }
            public string ReservedRootNamespaceNames { get; set; }
            public string NamespaceRentalFeeSinkAddressV1 { get; set; }
            public string NamespaceRentalFeeSinkAddress { get; set; }
            public string RootNamespaceRentalFeePerBlock { get; set; }
            public string ChildNamespaceRentalFee { get; set; }
        }

        public class RestrictionAccount
        {
            public string MaxAccountRestrictionValues { get; set; }
        }

        public class RestrictionMosaic
        {
            public string MaxMosaicRestrictionValues { get; set; }
        }

        public class Transfer
        {
            public string MaxMessageSize { get; set; }
        }

        public class Plugins
        {
            public AccountLink AccountLink { get; set; }
            public Aggregate Aggregate { get; set; }
            public LockHash LockHash { get; set; }
            public LockSecret LockSecret { get; set; }
            public Metadata Metadata { get; set; }
            public Mosaic Mosaic { get; set; }
            public Multisig Multisig { get; set; }
            public Namespace Namespace { get; set; }
            public RestrictionAccount RestrictionAccount { get; set; }
            public RestrictionMosaic RestrictionMosaic { get; set; }
            public Transfer Transfer { get; set; }
        }

        public class ForkHeights
        {
            public string TotalVotingBalanceCalculationFix { get; set; }
            public string TreasuryReissuance { get; set; }
            public string StrictAggregateTransactionHash { get; set; }
        }
        public class ChainConfig
        {
            public Network Network { get; set; }
            public Chain Chain { get; set; }
            public Plugins Plugins { get; set; }
            public ForkHeights ForkHeights { get; set; }
        }
    }
}
