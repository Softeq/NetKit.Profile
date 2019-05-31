// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.NetKit.Profile.Domain.Models.Profile;
using Softeq.NetKit.Profile.Domain.Repositories;

namespace Softeq.NetKit.Profile.SQLRepository.Repositories
{
    public class UserProfileRepository : RepositoryBase<UserProfile, Guid>, IUserProfileRepository
    {
        public UserProfileRepository(ApplicationDbContext dbFactory)
            : base(dbFactory)
        {
        }
    }
}