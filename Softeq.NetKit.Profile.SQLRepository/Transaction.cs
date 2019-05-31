// Developed by Softeq Development Corporation
// http://www.softeq.com

using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Softeq.NetKit.Profile.Domain.Infrastructure;

namespace Softeq.NetKit.Profile.SQLRepository
{
    public class Transaction : ITransaction
    {
        private readonly IDbContextTransaction _dbTransaction;

        public Transaction(DbContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();

            _dbTransaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        public void Commit()
        {
            _dbTransaction?.Commit();
        }

        public void Dispose()
        {
            _dbTransaction?.Dispose();
        }

        public void Rollback()
        {
            _dbTransaction.Rollback();
        }
    }
}