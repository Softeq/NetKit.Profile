// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.NetKit.Profile.Domain.Infrastructure;

namespace Softeq.NetKit.Profile.Domain.Models.Profile
{
    public class UserProfile : Entity<Guid>, ICreated, IUpdated
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Location { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public Gender Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Email { get; set; }

        public string PhotoName { get; set; }

        public string Bio { get; set; }

        public string UserId { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }
    }
}