using Autofac;
using Microsoft.Extensions.Logging;
using Moq;
using Softeq.CloudStorage.Extension;
using Softeq.NetKit.Profile.Domain.Infrastructure;
using Softeq.NetKit.Profile.Domain.Models.Configuration;
using Softeq.NetKit.ProfileService.Abstract;
using Softeq.NetKit.ProfileService.Services;

namespace Softeq.NetKit.Profile.Test.Unit
{
    public class AttachmentServiceTestsBase
    {
        protected readonly ILifetimeScope LifetimeScope;

        protected readonly IAttachmentService _attachmentService;
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
        protected readonly Mock<ILoggerFactory> _loggerFactoryMock = new Mock<ILoggerFactory>();
        protected readonly Mock<CloudStorageConfiguration> _cloudStorageConfigMock = new Mock<CloudStorageConfiguration>(MockBehavior.Strict, string.Empty, string.Empty, int.MinValue, string.Empty, int.MinValue);
        protected readonly Mock<IContentStorage> _contentStorageMock = new Mock<IContentStorage>(MockBehavior.Strict);

        protected AttachmentServiceTestsBase()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterAssemblyModules(typeof(ProfileServiceTestsBase).Assembly);
            LifetimeScope = containerBuilder.Build();

            _unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(It.IsAny<int>());

            _attachmentService = new AttachmentService(_contentStorageMock.Object, _cloudStorageConfigMock.Object, _loggerFactoryMock.Object);
        }

        protected void VerifyMocks()
        {
            _unitOfWorkMock.Verify();
        }
    }
}
