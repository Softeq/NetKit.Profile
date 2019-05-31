// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Profile.Domain.Repositories;

namespace Softeq.NetKit.Profile.Domain.Infrastructure
{
    public interface IRepositoryFactory
    {
        IUserProfileRepository UserProfileRepository { get; }
    }
}