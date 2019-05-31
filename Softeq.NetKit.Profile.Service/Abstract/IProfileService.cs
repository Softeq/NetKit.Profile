// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.NetKit.Profile.Domain.Models.Profile;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Queries;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Request;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Response;
using Softeq.NetKit.ProfileService.TransportModels.Shared;
using Softeq.QueryUtils;

namespace Softeq.NetKit.ProfileService.Abstract
{
    public interface IProfileService
    {
        Task<ProfileResponse> GetProfileAsync(ProfileRequest request);
        Task<ProfileResponse> CreateProfileAsync(CreateProfileAuthorizedRequest request);
        Task<ProfileResponse> UpdateProfileAsync(UpdateProfileRequest request);
        Task<UserProfile> GetProfileOrDefaultAsync(Guid profileId);
        Task<ProfileResponse> UpdateOtherProfileAsync(UpdateOtherProfileRequest request);
        Task<ProfileResponse> GetProfileAsync(UserRequest request);
        Task DeleteUserPhotoAsync(UserRequest request);
        Task DeleteOtherUserPhotoAsync(ProfileRequest request);
        Task UpdateUserPhotoAsync(UpdateUserPhotoRequest request);
        Task UpdateOtherUserPhotoAsync(UpdateOtherUserPhotoRequest request);
        Task<PagedResults<ProfileResponse>> GetProfilesAsync(GetProfilesQuery query);
        Task<bool> CheckIfProfileExistsAsync(string email);
    }
}