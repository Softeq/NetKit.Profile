// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.NetKit.Profile.Domain.Models.Profile;
using Softeq.NetKit.ProfileService.TransportModels.Shared;

namespace Softeq.NetKit.ProfileService.TransportModels.Profile.Request
{
    public class CreateProfileAuthorizedRequest : UserRequest
    {
        public CreateProfileAuthorizedRequest(string userId, string email) : base(userId)
        {
            Email = email;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Bio { get; set; }

        public string Location { get; set; }

        public Gender Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }
        
        public string Email { get; set; }
    }
}