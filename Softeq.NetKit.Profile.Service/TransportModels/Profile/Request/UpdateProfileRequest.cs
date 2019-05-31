// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.NetKit.Profile.Domain.Models.Profile;
using Softeq.NetKit.ProfileService.TransportModels.Shared;

namespace Softeq.NetKit.ProfileService.TransportModels.Profile.Request
{
    public class UpdateProfileRequest : UserRequest
    {
        public UpdateProfileRequest(string userId) : base(userId)
        {
        }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Bio { get; set; }
        
        public string Location { get; set; }

        public Gender Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}