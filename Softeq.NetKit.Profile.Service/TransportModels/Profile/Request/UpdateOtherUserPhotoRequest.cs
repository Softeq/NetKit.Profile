// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.ProfileService.TransportModels.Profile.Request
{
    public class UpdateOtherUserPhotoRequest
    {
        public UpdateOtherUserPhotoRequest(Guid requestedProfileId, string fileName)
        {
            RequestedProfileId = requestedProfileId;
            FileName = fileName;
        }

        public Guid RequestedProfileId { get; set; }
        public string FileName { get; set; }
    }
}
