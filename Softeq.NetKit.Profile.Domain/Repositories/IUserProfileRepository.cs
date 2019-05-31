// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.NetKit.Profile.Domain.Infrastructure;
using Softeq.NetKit.Profile.Domain.Models.Profile;

namespace Softeq.NetKit.Profile.Domain.Repositories
{
    public interface IUserProfileRepository : IRepository<UserProfile, Guid>
    {
    }
}