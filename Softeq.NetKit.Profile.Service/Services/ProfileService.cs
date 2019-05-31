// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using AutoMapper;
using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Softeq.CloudStorage.Extension;
using Softeq.CloudStorage.Extension.Exceptions;
using Softeq.NetKit.Profile.Domain.Infrastructure;
using Softeq.NetKit.Profile.Domain.Models.Configuration;
using Softeq.NetKit.Profile.Domain.Models.Profile;
using Softeq.NetKit.ProfileService.Abstract;
using Softeq.NetKit.ProfileService.Exceptions;
using Softeq.NetKit.ProfileService.Extensions;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Request;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Queries;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Response;
using Softeq.NetKit.ProfileService.TransportModels.Shared;
using Softeq.NetKit.ProfileService.Utility.ErrorHandling;
using Softeq.QueryUtils;

namespace Softeq.NetKit.ProfileService.Services
{
    public class ProfileService : BaseService, IProfileService
    {
        private readonly IContentStorage _contentStorage;
        private readonly CloudStorageConfiguration _configuration;

        public ProfileService(
            IUnitOfWork unitOfWork, 
            ILoggerFactory logger,
            IMapper mapper,
            IServiceProvider serviceProvider, 
            IContentStorage contentStorage, 
            CloudStorageConfiguration configuration) : base(unitOfWork, logger, mapper, serviceProvider)
        {
            _contentStorage = contentStorage;
            _configuration = configuration;
        }

        public async Task<ProfileResponse> CreateProfileAsync(CreateProfileAuthorizedRequest request)
        {
            Ensure.That(request, nameof(request)).IsNotNull();
            ValidateAndThrow(request);

            if (await CheckIfProfileExistsAsync(request.Email))
            {
                throw new ValidationException("A profile with the same email already exists in the system.");
            }

            var userProfile = await UnitOfWork.UserProfileRepository.Query(x => x.UserId == request.UserId)
                .FirstOrDefaultAsync();

            if (userProfile != null)
            {
                return Mapper.Map<UserProfile, ProfileResponse>(userProfile);
            }

            var newProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Bio = request.Bio,
                DateOfBirth = request.DateOfBirth,
                Location = request.Location,
                Gender = request.Gender,
                Email = request.Email
            };

            userProfile = UnitOfWork.UserProfileRepository.Add(newProfile);
            await UnitOfWork.SaveChangesAsync();
            return Mapper.Map<UserProfile, ProfileResponse>(userProfile);
        }

        public async Task<ProfileResponse> UpdateProfileAsync(UpdateProfileRequest request)
        {
            Ensure.That(request, nameof(request)).IsNotNull();
            ValidateAndThrow(request);

            var profile = await UnitOfWork.UserProfileRepository.Query(x => x.UserId == request.UserId)
                .FirstOrDefaultAsync();

            Ensure.Any.IsNotNull(profile, null, opts => opts.WithException(new NotFoundException(new ErrorDto(ErrorCode.NotFound, "Profile does not exist."))));

            profile.LastName = request.LastName;
            profile.FirstName = request.FirstName;
            profile.Bio = request.Bio;
            profile.Location = request.Location;
            profile.DateOfBirth = request.DateOfBirth?.Date;
            profile.Gender = request.Gender;
            profile.UserId = request.UserId;

            UnitOfWork.UserProfileRepository.Update(profile);
            await UnitOfWork.SaveChangesAsync();

            return Mapper.Map<UserProfile, ProfileResponse>(profile);
        }

        public async Task<ProfileResponse> UpdateOtherProfileAsync(UpdateOtherProfileRequest request)
        {
            Ensure.That(request, nameof(request)).IsNotNull();
            ValidateAndThrow(request);

            var profile = await UnitOfWork.UserProfileRepository.Query(x => x.Id == request.RequestedProfileId)
                .FirstOrDefaultAsync();

            Ensure.Any.IsNotNull(profile, null, opts => opts.WithException(new NotFoundException(new ErrorDto(ErrorCode.NotFound, "Profile does not exist."))));

            profile.LastName = request.LastName;
            profile.FirstName = request.FirstName;
            profile.Bio = request.Bio;
            profile.Location = request.Location;
            profile.DateOfBirth = request.DateOfBirth?.Date;
            profile.Gender = request.Gender;

            UnitOfWork.UserProfileRepository.Update(profile);
            await UnitOfWork.SaveChangesAsync();

            return Mapper.Map<UserProfile, ProfileResponse>(profile);
        }

        public async Task<UserProfile> GetProfileOrDefaultAsync(Guid profileId)
        {
            var profile = await UnitOfWork.UserProfileRepository.Query(x => x.Id == profileId)
                .FirstOrDefaultAsync();

            return profile;
        }

        public async Task<ProfileResponse> GetProfileAsync(ProfileRequest request)
        {
            var profile = await UnitOfWork.UserProfileRepository.Query(x => x.Id == request.CurrentProfileId)
                .FirstOrDefaultAsync();

            Ensure.Any.IsNotNull(profile, null, opts => opts.WithException(new NotFoundException(new ErrorDto(ErrorCode.NotFound, "Profile does not exist."))));

            return Mapper.Map<UserProfile, ProfileResponse>(profile);
        }

        public async Task<ProfileResponse> GetProfileAsync(UserRequest request)
        {
            var profile = await UnitOfWork.UserProfileRepository.Query(x => x.UserId == request.UserId)
                .FirstOrDefaultAsync();

            Ensure.Any.IsNotNull(profile, null, opts => opts.WithException(new NotFoundException(new ErrorDto(ErrorCode.NotFound, "Profile does not exist."))));

            return Mapper.Map<UserProfile, ProfileResponse>(profile);
        }

        public async Task DeleteUserPhotoAsync(UserRequest request)
        {
            var profile = await UnitOfWork.UserProfileRepository.Query(x => x.UserId == request.UserId)
                .FirstOrDefaultAsync();

            Ensure.Any.IsNotNull(profile, null, opts => opts.WithException(new NotFoundException(new ErrorDto(ErrorCode.NotFound, "Profile does not exist."))));

            if (string.IsNullOrWhiteSpace(profile.PhotoName))
            {
                return;
            }

            var fileName = profile.PhotoName;

            await _contentStorage.DeleteContentAsync(fileName, _configuration.UserPhotoContainerName);

            profile.PhotoName = null;
            UnitOfWork.UserProfileRepository.Update(profile);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task DeleteOtherUserPhotoAsync(ProfileRequest request)
        {
            var profile = await UnitOfWork.UserProfileRepository.Query(x => x.Id == request.CurrentProfileId)
                .FirstOrDefaultAsync();

            Ensure.Any.IsNotNull(profile, null, opts => opts.WithException(new NotFoundException(new ErrorDto(ErrorCode.NotFound, "Profile does not exist."))));

            if (string.IsNullOrWhiteSpace(profile.PhotoName))
            {
                return;
            }

            var fileName = profile.PhotoName;

            await _contentStorage.DeleteContentAsync(fileName, _configuration.UserPhotoContainerName);

            profile.PhotoName = null;
            UnitOfWork.UserProfileRepository.Update(profile);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task UpdateUserPhotoAsync(UpdateUserPhotoRequest request)
        {
            Ensure.That(request, nameof(request)).IsNotNull();
            ValidateAndThrow(request);

            var profile = await UnitOfWork.UserProfileRepository.Query(x => x.UserId == request.UserId)
                .FirstOrDefaultAsync();

            Ensure.Any.IsNotNull(profile, null, opts => opts.WithException(new NotFoundException(new ErrorDto(ErrorCode.NotFound, "Profile does not exist."))));

            try
            {
                await DeleteUserPhotoAsync(new UserRequest(request.UserId));
                await _contentStorage.CopyBlobAsync(request.FileName, _configuration.TempContainerName, _configuration.UserPhotoContainerName);
            }
            catch (ContentStorageException ex)
            {
                Log.LogError(ex.Message, ex);
                throw new ServiceException(new ErrorDto(ErrorCode.StorageError, ex.Message));
            }
            catch (StorageException ex)
            {
                Log.LogError(ex.Message, ex);
                throw new ServiceException(new ErrorDto(ErrorCode.StorageError, ex.Message));
            }

            profile.PhotoName = request.FileName;
            UnitOfWork.UserProfileRepository.Update(profile);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task UpdateOtherUserPhotoAsync(UpdateOtherUserPhotoRequest request)
        {
            Ensure.That(request, nameof(request)).IsNotNull();
            ValidateAndThrow(request);

            var profile = await UnitOfWork.UserProfileRepository.Query(x => x.Id == request.RequestedProfileId)
                .FirstOrDefaultAsync();

            Ensure.Any.IsNotNull(profile, null, opts => opts.WithException(new NotFoundException(new ErrorDto(ErrorCode.NotFound, "Profile does not exist."))));

            try
            {
                await DeleteUserPhotoAsync(new UserRequest(profile.UserId));
                await _contentStorage.CopyBlobAsync(request.FileName, _configuration.TempContainerName, _configuration.UserPhotoContainerName);
            }
            catch (ContentStorageException ex)
            {
                Log.LogError(ex.Message, ex);
                throw new ServiceException(new ErrorDto(ErrorCode.StorageError, ex.Message));
            }

            profile.PhotoName = request.FileName;
            UnitOfWork.UserProfileRepository.Update(profile);
            await UnitOfWork.SaveChangesAsync();
        }

        public PagedResults<ProfileResponse> GetProfiles(GetProfilesQuery query)
        {
            var filters = query.CreateFilters();
            var ordering = query.CreateOrdering();

            var querySpecification = new QuerySpecification<UserProfile>()
                .WithFilters(filters)
                .WithOrdering(ordering);

            var profiles = UnitOfWork.UserProfileRepository.GetAll();

            var filteredProfiles = querySpecification.ApplyFiltering(profiles);

            return PageUtil.CreatePagedResults(filteredProfiles, query.Page, query.PageSize, Mapper.Map<UserProfile, ProfileResponse>);
        }

        public async Task<bool> CheckIfProfileExistsAsync(string email)
        {
            return await UnitOfWork.UserProfileRepository.Query(x => x.Email == email)
                .AnyAsync();
        }
    }
}
