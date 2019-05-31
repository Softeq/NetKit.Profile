// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Softeq.NetKit.Profile.Domain.Models.Profile;
using Softeq.NetKit.ProfileService.Exceptions;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Queries;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Response;
using Softeq.QueryUtils;

namespace Softeq.NetKit.ProfileService.Extensions
{
    public static class ProfileQueryExtensions
    {
        public static IEnumerable<Func<IQueryable<UserProfile>, IQueryable<UserProfile>>> CreateFilters(this GetProfilesQuery query)
        {
            var filters = new List<Func<IQueryable<UserProfile>, IQueryable<UserProfile>>>();

            var map = typeof(ProfileResponse).GetProperties()
                .Where(x => x.GetCustomAttribute<JsonPropertyAttribute>() != null)
                .ToLookup(x => x.GetCustomAttribute<JsonPropertyAttribute>().PropertyName);

            if (query.Filters != null)
            {
                foreach (var filter in query.Filters)
                {
                    var property = map[filter.PropertyName].FirstOrDefault();
                    if (property == null)
                    {
                        throw new QueryException($"Could not find '{filter.PropertyName}' in model scheme.");
                    }

                    switch (property.Name)
                    {
                        case nameof(ProfileResponse.FirstName):
                        {
                            filters.Add(x => x.Where(e => e.FirstName.Contains(filter.Value)));
                            break;
                        }
                        case nameof(ProfileResponse.LastName):
                        {
                            filters.Add(x => x.Where(e => e.LastName.Contains(filter.Value)));
                            break;
                        }
                        case nameof(ProfileResponse.Email):
                        {
                            filters.Add(x => x.Where(e => e.Email.Contains(filter.Value)));
                            break;
                        }
                        case nameof(ProfileResponse.DateOfBirth):
                        {
                            filters.Add(x => x.Where(e => e.DateOfBirth.ToString().Contains(filter.Value)));
                            break;
                        }
                        case nameof(ProfileResponse.Created):
                        {
                            filters.Add(x => x.Where(e => e.Created.ToString().Contains(filter.Value)));
                            break;
                        }
                        case nameof(ProfileResponse.Location):
                        {
                            filters.Add(x => x.Where(e => e.Location.Contains(filter.Value)));
                            break;
                        }
                        case nameof(ProfileResponse.Gender):
                        {
                            filters.Add(x => x.Where(e => e.Gender.ToString() == filter.Value));
                            break;
                        }
                        case nameof(ProfileResponse.Bio):
                        {
                            filters.Add(x => x.Where(e => e.Bio.Contains(filter.Value)));
                            break;
                        }
                        case nameof(ProfileResponse.Updated):
                        {
                            filters.Add(x => x.Where(e => e.Updated.ToString().Contains(filter.Value)));
                            break;
                        }
                        default:
                        {
                            throw new QueryException($"Unsupported property filtering. Property name: '{filter.PropertyName}'.");
                        }
                    }
                }
            }

            return filters;
        }

        public static Func<IQueryable<UserProfile>, IQueryable<UserProfile>> CreateOrdering(this GetProfilesQuery query)
        {
            Func<IQueryable<UserProfile>, IQueryable<UserProfile>> order = null;

            if (query.Sort != null)
            {
                var map = typeof(ProfileResponse).GetProperties()
                    .Where(x => x.GetCustomAttribute<JsonPropertyAttribute>() != null)
                    .ToLookup(x => x.GetCustomAttribute<JsonPropertyAttribute>().PropertyName);

                var property = map[query.Sort.PropertyName].FirstOrDefault();
                if (property == null)
                {
                    throw new QueryException($"Could not find '{query.Sort.PropertyName}' in model scheme.");
                }

                Expression<Func<UserProfile, object>> sortExpression;

                switch (property.Name)
                {
                    case nameof(ProfileResponse.FirstName):
                    {
                        sortExpression = x => x.FirstName;
                        break;
                    }
                    case nameof(ProfileResponse.LastName):
                    {
                        sortExpression = x => x.LastName;
                        break;
                    }
                    case nameof(ProfileResponse.Email):
                    {
                        sortExpression = x => x.Email;
                        break;
                    }
                    case nameof(ProfileResponse.DateOfBirth):
                    {
                        sortExpression = x => x.DateOfBirth;
                        break;
                    }
                    case nameof(ProfileResponse.Created):
                    {
                        sortExpression = x => x.Created;
                        break;
                    }
                    case nameof(ProfileResponse.Location):
                    {
                        sortExpression = x => x.Location;
                        break;
                    }
                    case nameof(ProfileResponse.Gender):
                    {
                        sortExpression = x => x.Gender;
                        break;
                    }
                    case nameof(ProfileResponse.Bio):
                    {
                        sortExpression = x => x.Bio;
                        break;
                    }
                    case nameof(ProfileResponse.Updated):
                    {
                        sortExpression = x => x.Updated;
                        break;
                    }
                    default:
                    {
                        throw new QueryException($"Unsupported property filtering. Property name: '{query.Sort.PropertyName}'.");
                    }
                }

                order = query.Sort.Order == SortOrder.Asc
                    ? (Func<IQueryable<UserProfile>, IQueryable<UserProfile>>) (x => x.OrderBy(sortExpression))
                    : (x => x.OrderByDescending(sortExpression));
            }

            return order;
        }
    }
}