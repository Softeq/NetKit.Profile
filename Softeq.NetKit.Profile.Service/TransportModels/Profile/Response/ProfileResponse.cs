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
        [JsonProperty("profileId")]
        public Guid ProfileId { get; set; }

        [MaxLength(100)]
        [Display(Name = "First Name")]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Display(Name = "Last Name")]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("gender")]
        public Gender? Gender { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("photoUrl")]
        public string PhotoUrl { get; set; }

        [Display(Name = "Registration Date")]
        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [Display(Name = "Updated")]
        [JsonProperty("updated")]
        public DateTime? Updated { get; set; }
    }
}