// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.NetKit.Profile.Domain.Infrastructure;
using Softeq.NetKit.Profile.Domain.Repositories;
using Softeq.NetKit.Profile.SQLRepository.Repositories;

namespace Softeq.NetKit.Profile.SQLRepository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        protected ApplicationDbContext DbContext;

        public UnitOfWork(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync(true);
        }

        public ITransaction BeginTransaction()
        {
            return new Transaction(DbContext);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }

        #region Repositories

        private IUserProfileRepository _userProfileRepository;
        public IUserProfileRepository UserProfileRepository => _userProfileRepository ?? (_userProfileRepository = new UserProfileRepository(DbContext));

        #endregion
    }
}