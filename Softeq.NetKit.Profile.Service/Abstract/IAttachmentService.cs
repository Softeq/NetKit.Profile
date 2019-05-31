// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Request;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Response;

namespace Softeq.NetKit.ProfileService.Abstract
{
    public interface IAttachmentService
    {
        Task<string> UploadAttachmentAsync(byte[] attachmentBytes, string contentType, string fileName);
        Task<bool> AttachmentExistsAsync(string fileName, string containerName);
        Task<string> GetBlobSasUriAsync(GetBlobSasUriRequest request);
        Task<ContainerAccessTokenResponse> GetContainerAccessKeyAsync();
    }
}
