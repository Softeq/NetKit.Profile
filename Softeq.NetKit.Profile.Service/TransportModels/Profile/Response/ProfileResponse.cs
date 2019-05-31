// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Softeq.NetKit.Profile.Domain.Models.Profile;

namespace Softeq.NetKit.ProfileService.TransportModels.Profile.Response
{
    public class ProfileResponse
    {
        public Guid ProfileId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public Gender? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string Location { get; set; }

        public string PhotoUrl { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }
    }
}