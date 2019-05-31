// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.Extensions.Logging;
using Softeq.CloudStorage.Extension;
using Softeq.CloudStorage.Extension.Exceptions;
using Softeq.NetKit.Profile.Domain.Models.Configuration;
using Softeq.NetKit.ProfileService.Abstract;
using Softeq.NetKit.ProfileService.Exceptions;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Request;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Response;
using Softeq.NetKit.ProfileService.Utility.ErrorHandling;

namespace Softeq.NetKit.ProfileService.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IContentStorage _contentStorage;
        private readonly CloudStorageConfiguration _configuration;
        private readonly ILogger _logger;

        public AttachmentService(
            IContentStorage contentStorage, 
            CloudStorageConfiguration configuration, 
            ILoggerFactory logger)
        {
            Ensure.That(logger, nameof(logger)).IsNotNull();
            Ensure.That(contentStorage, nameof(contentStorage)).IsNotNull();
            Ensure.That(configuration, nameof(configuration)).IsNotNull();

            _contentStorage = contentStorage;
            _configuration = configuration;

            _logger = logger.CreateLogger(GetType());
        }

        public async Task<string> UploadAttachmentAsync(byte[] attachmentBytes, string contentType, string fileName)
        {
            Uri uploadedFilePath = null;

            try
            {
                if (attachmentBytes != null && !string.IsNullOrEmpty(fileName))
                {
                    using (Stream stream = new MemoryStream(attachmentBytes))
                    {
                        uploadedFilePath = await _contentStorage.SaveContentAsync(fileName, stream, _configuration.TempContainerName, contentType);
                    }
                }
            }
            catch (ContentStorageException ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ServiceException(new ErrorDto(ErrorCode.StorageError, ex.Message));
            }

            return !string.IsNullOrEmpty(uploadedFilePath?.ToString()) ? $"{uploadedFilePath}" : null;
        }

        public async Task<bool> AttachmentExistsAsync(string fileName, string containerName)
        {
            return await _contentStorage.BlobExistsAsync(fileName, containerName);
        }

        public async Task<string> GetBlobSasUriAsync(GetBlobSasUriRequest request)
        {
            var blobSasUri = await _contentStorage.GetBlobSasUriAsync(request.ContainerName, request.BlobName, _configuration.AccessTokenExpiration);
            return blobSasUri;
        }

        public async Task<ContainerAccessTokenResponse> GetContainerAccessKeyAsync()
        {
            var containerAccessToken = await _contentStorage.GetContainerSasTokenAsync(_configuration.TempContainerName, _configuration.AccessTokenExpiration);

            var response = new ContainerAccessTokenResponse
            {
                ContentStorageHost = _configuration.ContentStorageHost,
                AccessToken = containerAccessToken,
                ContainerName = _configuration.TempContainerName
            };

            return response;
        }
    }
}
