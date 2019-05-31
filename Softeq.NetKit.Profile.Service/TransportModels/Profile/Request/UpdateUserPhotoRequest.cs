// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.ProfileService.TransportModels.Shared;

namespace Softeq.NetKit.ProfileService.TransportModels.Profile.Request
{
    public class UpdateUserPhotoRequest : UserRequest
    {
        public UpdateUserPhotoRequest(string userId) : base(userId)
        {
        }

        public string FileName { get; set; }
    }
}