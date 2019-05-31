using System;
using Autofac;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Softeq.CloudStorage.Extension;
using Softeq.NetKit.Profile.Domain.Infrastructure;
using Softeq.NetKit.Profile.Domain.Models.Configuration;
using Softeq.NetKit.ProfileService.Abstract;

namespace Softeq.NetKit.Profile.Test.Unit
{
    public class ProfileServiceTestsBase
    {
        protected readonly ILifetimeScope LifetimeScope;

        protected readonly IProfileService _profileService;
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
        protected readonly Mock<ILoggerFactory> _loggerFactoryMock = new Mock<ILoggerFactory>();
        protected readonly Mock<CloudStorageConfiguration> _cloudStorageConfigMock = new Mock<CloudStorageConfiguration>(MockBehavior.Strict, string.Empty, string.Empty, int.MinValue, string.Empty, int.MinValue);
        protected readonly Mock<IContentStorage> _contentStorageMock = new Mock<IContentStorage>(MockBehavior.Strict);

        protected ProfileServiceTestsBase()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterAssemblyModules(typeof(ProfileServiceTestsBase).Assembly);
            LifetimeScope = containerBuilder.Build();

            var serviceProvider = LifetimeScope.Resolve<IServiceProvider>();
            var mapper = LifetimeScope.Resolve<IMapper>();

            _unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(It.IsAny<int>());

            _profileService = new ProfileService.Services.ProfileService(_unitOfWorkMock.Object, _loggerFactoryMock.Object, mapper, serviceProvider, _contentStorageMock.Object, _cloudStorageConfigMock.Object);
        }

        protected void VerifyMocks()
        {
            _unitOfWorkMock.Verify();
        }
    }
}
