// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.Profile.Domain.Models.Configuration
{
    public class CloudStorageConfiguration
    {
        public string ContentStorageHost { get; set; }
        public string TempContainerName { get; set; }
        public string UserPhotoContainerName { get; set; }
        public int UserPhotoSize { get; set; }
        public int AccessTokenExpiration { get; set; }

        public CloudStorageConfiguration(
            string contentStorageHost,
            string userPhotoContainerName,
            int userPhotoSize,
            string tempContainerName,
            int accessTokenExpiration)
        {
            ContentStorageHost = contentStorageHost;
            UserPhotoContainerName = userPhotoContainerName;
            UserPhotoSize = userPhotoSize;
            TempContainerName = tempContainerName;
            AccessTokenExpiration = accessTokenExpiration;
        }

        public string GetUrl(string fileName, string container)
        {
            return new Uri($"{ContentStorageHost}/{container}/{fileName}").ToString();
        }
    }
}