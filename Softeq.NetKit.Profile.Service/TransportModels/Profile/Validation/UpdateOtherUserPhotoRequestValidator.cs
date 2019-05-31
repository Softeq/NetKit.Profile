// Developed by Softeq Development Corporation
// http://www.softeq.com

using FluentValidation;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Request;

namespace Softeq.NetKit.ProfileService.TransportModels.Profile.Validation
{
    public class UpdateOtherUserPhotoRequestValidator : AbstractValidator<UpdateOtherUserPhotoRequest>
    {
        public UpdateOtherUserPhotoRequestValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty();

            RuleFor(x => x.RequestedProfileId)
                .NotEmpty();
        }
    }
}