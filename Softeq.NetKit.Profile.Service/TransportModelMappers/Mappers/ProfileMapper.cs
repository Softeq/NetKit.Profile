// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Profile.Domain.Models.Profile;
using Softeq.NetKit.ProfileService.TransportModelMappers.Resolvers;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Response;

namespace Softeq.NetKit.ProfileService.TransportModelMappers.Mappers
{
    public class ProfileMapper : AutoMapper.Profile
    {
        public ProfileMapper()
        {
            CreateMap<UserProfile, ProfileResponse>()
                .ForMember(x => x.ProfileId, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.Email, cfg => cfg.MapFrom(x => x.Email))
                .ForMember(x => x.FullName, cfg => cfg.MapFrom(x => x.FirstName + " " + x.LastName))
                .ForMember(x => x.DateOfBirth, cfg => cfg.MapFrom(x => x.DateOfBirth))
                .ForMember(x => x.Created, cfg => cfg.MapFrom(x => x.Created))
                .ForMember(x => x.PhotoUrl, cfg => cfg.ResolveUsing<ProfilePhotoUrlResolver, string>(src => src.PhotoName));
        }
    }
}