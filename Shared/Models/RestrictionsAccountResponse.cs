using System.Collections.Generic;

namespace NageXymSharpApps.Shared.Models
{
    public class RestrictionsAccountResponse
    {
        public List<object> Data { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class Pagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}