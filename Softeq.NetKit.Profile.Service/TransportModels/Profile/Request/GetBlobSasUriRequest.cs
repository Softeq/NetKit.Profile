// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.ProfileService.TransportModels.Profile.Request
{
    public class GetBlobSasUriRequest
    {
        public GetBlobSasUriRequest(string containerName, string blobName)
        {
            ContainerName = containerName;
            BlobName = blobName;
        }

        public string ContainerName { get; }
        public string BlobName { get; }
    }
}