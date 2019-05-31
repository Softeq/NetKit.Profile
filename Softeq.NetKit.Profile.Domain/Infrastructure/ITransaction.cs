// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Profile.Domain.Infrastructure
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}