namespace NageXymSharpApps.Client.Models
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

    public class Alias
    {
        public int Type { get; set; }
        public string Address { get; set; }
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
