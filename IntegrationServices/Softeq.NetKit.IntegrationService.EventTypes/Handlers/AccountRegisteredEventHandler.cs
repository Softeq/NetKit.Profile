// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.NetKit.Components.EventBus.Abstract;
using Softeq.NetKit.IntegrationService.EventTypes.Events;
using Softeq.NetKit.ProfileService.Abstract;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Request;

namespace Softeq.NetKit.IntegrationService.EventTypes.Handlers
{
    public class AccountRegisteredEventHandler : IEventHandler<AccountRegisteredEvent>
    {
        private readonly IProfileService _profileService;

        public AccountRegisteredEventHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task Handle(AccountRegisteredEvent @event)
        {
            var model = new CreateProfileAuthorizedRequest(@event.UserId, @event.Email)
            {
                FirstName = @event.FirstName,
                LastName = @event.LastName,
                Bio = @event.Bio,
                DateOfBirth = @event.DateOfBirth,
                Gender = @event.Gender,
                Location = @event.Location
            };

            await _profileService.CreateProfileAsync(model);
        }
    }
}