// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.NetKit.ProfileService.TransportModels.Profile.Request
{
    public class ProfileRequest
    {
        public ProfileRequest(Guid currentProfileId)
        {
            CurrentProfileId = currentProfileId;
        }

        public Guid CurrentProfileId { get; set; }
    }
}