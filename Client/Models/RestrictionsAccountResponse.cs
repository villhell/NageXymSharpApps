namespace NageXymSharpApps.Client.Models
{
    public class RestrictionsAccountResponse
    {
        public List<AccountRestriction> Data { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class AccountRestriction
    {
        public AccountRestrictions AccountRestrictions { get; set; }
    }

    public class AccountRestrictions
    {
        public int Version { get; set; }
        public string Address { get; set; }
        public List<Restriction> Restrictions { get; set; }
    }

    public class Restriction
    {
        public int RestrictionFlags { get; set; }
        public List<string> Values { get; set; }
    }

    public class Pagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}