using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Softeq.NetKit.Profile.Test.Unit
{
    public class AttachmentServiceTests : AttachmentServiceTestsBase
    {
        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldGetContainerAccessKey()
        {
            string sasToken = "receivedSasToken";

            _contentStorageMock.Setup(x => x.GetContainerSasTokenAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(sasToken);

            // Act
            var containerAccessToken = await _attachmentService.GetContainerAccessKeyAsync();

            // Assert
            Assert.NotNull(containerAccessToken);
            Assert.Equal(containerAccessToken.AccessToken, sasToken);

            VerifyMocks();
        }
    }
}
