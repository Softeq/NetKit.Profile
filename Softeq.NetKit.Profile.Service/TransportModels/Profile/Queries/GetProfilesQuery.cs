// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.QueryUtils;

namespace Softeq.NetKit.ProfileService.TransportModels.Profile.Queries
{
    public class GetProfilesQuery : IPagedQuery, IFilteredQuery, ISortedQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public ICollection<Filter> Filters { get; set; }
        public Sort Sort { get; set; }
    }
}