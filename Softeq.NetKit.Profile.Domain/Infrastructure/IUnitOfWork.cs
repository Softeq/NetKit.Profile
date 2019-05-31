// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.NetKit.Profile.Domain.Infrastructure
{
    public interface IUnitOfWork : IRepositoryFactory
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        ITransaction BeginTransaction();
    }
}