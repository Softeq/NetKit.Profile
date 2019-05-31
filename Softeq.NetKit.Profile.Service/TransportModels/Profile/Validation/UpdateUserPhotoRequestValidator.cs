// Developed by Softeq Development Corporation
// http://www.softeq.com

using FluentValidation;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Request;

namespace Softeq.NetKit.ProfileService.TransportModels.Profile.Validation
{
    public class UpdateUserPhotoRequestValidator : AbstractValidator<UpdateUserPhotoRequest>
    {
        public UpdateUserPhotoRequestValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty();
        }
    }
}