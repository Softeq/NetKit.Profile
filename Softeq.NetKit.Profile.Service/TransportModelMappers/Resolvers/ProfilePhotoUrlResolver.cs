// Developed by Softeq Development Corporation
// http://www.softeq.com

using AutoMapper;
using Softeq.CloudStorage.Extension;
using Softeq.NetKit.Profile.Domain.Models.Configuration;

namespace Softeq.NetKit.ProfileService.TransportModelMappers.Resolvers
{
    public class ProfilePhotoUrlResolver : IMemberValueResolver<object, object, string, string>
    {
        private readonly CloudStorageConfiguration _configuration;
        private readonly IContentStorage _contentStorage;

        public ProfilePhotoUrlResolver()
        {
        }

        public ProfilePhotoUrlResolver(CloudStorageConfiguration configuration, IContentStorage contentStorage)
        {
            _configuration = configuration;
            _contentStorage = contentStorage;
        }

        public string Resolve(object source, object destination, string sourceMember, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(sourceMember))
            {
                return null;
            }

            //TODO try to do async
            var blobSasUri = _contentStorage.GetBlobSasUriAsync(_configuration.UserPhotoContainerName, sourceMember, _configuration.AccessTokenExpiration).GetAwaiter().GetResult();
            return blobSasUri;
        }
    }
}