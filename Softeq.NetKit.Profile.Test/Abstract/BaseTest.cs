// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Softeq.NetKit.Profile.Test.Data;

namespace Softeq.NetKit.Profile.Test.Abstract
{
    public abstract class BaseTest : IDisposable
    {
        protected readonly ILifetimeScope LifetimeScope;

        protected BaseTest()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterAssemblyModules(typeof(BaseTest).Assembly);
            LifetimeScope = containerBuilder.Build();
            var seeder = LifetimeScope.Resolve<DbSeeder>();
            seeder.Seed().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            var dbContext = LifetimeScope.Resolve<DbContext>();
            dbContext.Dispose();
            LifetimeScope?.Dispose();
        }
    }
}