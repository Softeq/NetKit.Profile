// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Softeq.NetKit.Profile.SQLRepository;
using Softeq.NetKit.Profile.Test.Data;

namespace Softeq.NetKit.Profile.Test.Dependency
{
    public class DbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                {
                    //// Register dbContext to local database
                    //var config = context.Resolve<IConfigurationRoot>();
                    //var dbBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                    //    .UseSqlServer(config["ConnectionStrings:DefaultConnection"]);
                    
                    // Register dbContext to database in memory
                    var dbBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                    var dbContext = new ApplicationDbContext(dbBuilder.Options);
                    return dbContext;
                })
                .As<DbContext>()
                .AsSelf()
                .SingleInstance();

            builder.Register(x => new UnitOfWork(x.Resolve<ApplicationDbContext>()))
                .AsImplementedInterfaces();

            builder.RegisterType<DbSeeder>()
                .AsSelf();
        }
    }
}