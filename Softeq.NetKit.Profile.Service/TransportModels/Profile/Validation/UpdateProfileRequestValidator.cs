// Developed by Softeq Development Corporation
// http://www.softeq.com

using FluentValidation;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Request;

namespace Softeq.NetKit.ProfileService.TransportModels.Profile.Validation
{
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.Bio)
                .MaximumLength(300);

            RuleFor(x => x.Location)
                .MaximumLength(100);
        }
    }
}