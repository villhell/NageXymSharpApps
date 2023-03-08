namespace NageXymSharpApps.Shared.Models
{
    public class NamespaceResponse
    {
        public Meta Meta { get; set; }
        public Namespace Namespace { get; set; }
        public string Id { get; set; }

    }
    public class Meta
    {
        public int Index { get; set; }
        public bool Active { get; set; }
    }

    /// <summary>
    /// Alias
    /// </summary>
    public class Alias
    {
        /// <summary>
        /// 1 : Mosaic
        /// 2 : Address
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// MosaicId
        /// </summary>
        public string MosaicId { get; set; }
    }

    public class Namespace
    {
        public int Version { get; set; }
        public int RegistrationType { get; set; }
        public int Depth { get; set; }
        public string Level0 { get; set; }
        public Alias Alias { get; set; }
        public string ParentId { get; set; }
        public string OwnerAddress { get; set; }
        public string StartHeight { get; set; }
        public string EndHeight { get; set; }
    }
}
