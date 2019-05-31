// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.NetKit.Components.EventBus.Events;
using Softeq.NetKit.Profile.Domain.Models.Profile;

namespace Softeq.NetKit.IntegrationService.EventTypes.Events
{
    public class AccountRegisteredEvent : IntegrationEvent
    {
        public string UserId { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Location { get; }
        public string Bio { get; }
        public DateTime? DateOfBirth { get; }
        public Gender Gender { get; }

        public AccountRegisteredEvent(string userId, string email, string firstName, string lastName, string bio, string location, Gender gender, DateTime? dateOfBirth)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Location = location;
            Bio = bio;
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }
    }
}