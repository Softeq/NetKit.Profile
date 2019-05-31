// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Softeq.NetKit.Profile.Domain.Models.Configuration;
using Softeq.NetKit.Profile.Domain.Models.Profile;
using Softeq.NetKit.Profile.Test.Abstract;
using Softeq.NetKit.ProfileService.Abstract;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Queries;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Request;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Response;
using Softeq.NetKit.ProfileService.TransportModels.Shared;
using Softeq.NetKit.ProfileService.Utility;
using Softeq.QueryUtils;
using Xunit;

namespace Softeq.NetKit.Profile.Test
{
    public class ProfileServiceTests : BaseTest
    {
        private const string UserId = "cd598ab3-2bf2-42ff-bfe0-0d42ed7fe89c";
        private const string Email = "test@test.test";
        private const string ProfileId = "9DBB5AE0-6F75-41D5-A06C-F890983B0A64";
        private readonly IProfileService _profileService;
        private readonly IAttachmentService _attachmentService;
        private readonly CloudStorageConfiguration _cloudStorageConfiguration;

        public ProfileServiceTests()
        {
            _profileService = LifetimeScope.Resolve<IProfileService>();
            _attachmentService = LifetimeScope.Resolve<IAttachmentService>();
            _cloudStorageConfiguration = LifetimeScope.Resolve<CloudStorageConfiguration>();
        }

        [Fact]
        public async Task ShouldGetAllProfiles()
        {
            var patients = await _profileService.GetProfilesAsync(new GetProfilesQuery
            {
                Page = 1,
                PageSize = 10,
                Filters = new List<Filter>
                { 
                    new Filter
                    {
                        PropertyName = "email",
                        Value = "d"
                    }
                },
                Sort = new Sort
                { 
                    PropertyName = "email",
                    Order = SortOrder.Asc
                }
            });
            Assert.NotNull(patients.Results);
        }

        [Fact]
        public async Task ShouldGetUserProfile()
        {
            var profile = new CreateProfileAuthorizedRequest(UserId, Email)
            {
                Bio = "Test",
                FirstName = "Test",
                LastName = "Test",
                Gender = Gender.Male,
                Location = "Test",
                DateOfBirth = new DateTime()
            };
            await _profileService.CreateProfileAsync(profile);
            var newProfile = await _profileService.GetProfileAsync(new UserRequest(UserId));
            Assert.IsType<ProfileResponse>(newProfile);
            Assert.NotNull(newProfile);
        }

        [Fact]
        public async Task ShouldCheckUserProfileExists()
        {
            var profile = new CreateProfileAuthorizedRequest(UserId, Email)
            {
                Bio = "Test",
                FirstName = "Test",
                LastName = "Test",
                Gender = Gender.Male,
                Location = "Test",
                DateOfBirth = new DateTime()
            };
            await _profileService.CreateProfileAsync(profile);

            bool isProfileExists = await _profileService.CheckIfProfileExistsAsync(profile.Email);
            Assert.True(isProfileExists);
        }

        [Fact]
        public async Task ShouldUpdateUserProfile()
        {
            var profile = new CreateProfileAuthorizedRequest(UserId, Email)
            {
                Bio = "Test",
                FirstName = "Test",
                LastName = "Test",
                Gender = Gender.Male,
                Location = "Test",
                DateOfBirth = new DateTime()
            };
            await _profileService.CreateProfileAsync(profile);
            var updatedProfile = new UpdateProfileRequest(UserId)
            {
                DateOfBirth = new DateTime(),
                FirstName = "test",
                LastName = "test",
                Gender = Gender.Female,
                Bio = "test_bio",
                Location = "USA",
            };
            var newProfile = await _profileService.UpdateProfileAsync(updatedProfile);
            Assert.IsType<ProfileResponse>(newProfile);
            Assert.NotNull(newProfile);
            Assert.True(newProfile.Location == updatedProfile.Location);
            Assert.True(newProfile.LastName == updatedProfile.LastName);
            Assert.True(newProfile.DateOfBirth == updatedProfile.DateOfBirth);
            Assert.True(newProfile.FirstName == updatedProfile.FirstName);
            Assert.True(newProfile.Gender == updatedProfile.Gender);
            Assert.True(newProfile.Bio == updatedProfile.Bio);
            Assert.True(newProfile.FullName == updatedProfile.FirstName + " " + updatedProfile.LastName);
        }

        [Fact]
        public async Task ShouldUpdateOtherUserProfile()
        {
            var model = new CreateProfileAuthorizedRequest(UserId, Email)
            {
                Bio = "Test",
                FirstName = "Test",
                LastName = "Test",
                Gender = Gender.Male,
                Location = "Test",
                DateOfBirth = new DateTime()
            };
            var profile = await _profileService.CreateProfileAsync(model);
            var updatedProfile = new UpdateOtherProfileRequest
            {
                RequestedProfileId = profile.ProfileId,
                DateOfBirth = new DateTime(),
                FirstName = "test",
                LastName = "test",
                Gender = Gender.Female,
                Bio = "test_bio",
                Location = "USA",
            };
            var newProfile = await _profileService.UpdateOtherProfileAsync(updatedProfile);
            Assert.IsType<ProfileResponse>(newProfile);
            Assert.NotNull(newProfile);
            Assert.True(newProfile.Location == updatedProfile.Location);
            Assert.True(newProfile.LastName == updatedProfile.LastName);
            Assert.True(newProfile.DateOfBirth == updatedProfile.DateOfBirth);
            Assert.True(newProfile.FirstName == updatedProfile.FirstName);
            Assert.True(newProfile.Gender == updatedProfile.Gender);
            Assert.True(newProfile.Bio == updatedProfile.Bio);
            Assert.True(newProfile.FullName == updatedProfile.FirstName + " " + updatedProfile.LastName);
        }

        [Fact]
        public async Task ShouldCreateUserProfile()
        {
            var profile = new CreateProfileAuthorizedRequest(Guid.NewGuid().ToString(), Email)
            {
                Bio = "Test",
                FirstName = "Test",
                LastName = "Test",
                Gender = Gender.Male,
                Location = "Test",
                DateOfBirth = new DateTime()
            };
            var newProfile = await _profileService.CreateProfileAsync(profile);
            Assert.IsType<ProfileResponse>(newProfile);
            Assert.NotNull(newProfile);
            Assert.True(profile.Bio == newProfile.Bio);
            Assert.True(profile.DateOfBirth == newProfile.DateOfBirth);
            Assert.True(profile.FirstName == newProfile.FirstName);
            Assert.True(profile.LastName == newProfile.LastName);
            Assert.True(profile.Gender == newProfile.Gender);
            Assert.True(profile.Location == newProfile.Location);
            Assert.True(newProfile.FullName == profile.FirstName + " " + profile.LastName);
        }

        [Fact]
        public async Task ShouldUpdateUserPhotoAsync()
        {
            var model = new CreateProfileAuthorizedRequest(Guid.NewGuid().ToString(), Email)
            {
                Bio = "Test",
                FirstName = "Test",
                LastName = "Test",
                Gender = Gender.Male,
                Location = "Test",
                DateOfBirth = new DateTime()
            };
            var profile = await _profileService.CreateProfileAsync(model);
            var newProfile = await _profileService.GetProfileOrDefaultAsync(profile.ProfileId);

            var fileExtension = "jpg";
            var fileName = $"{Guid.NewGuid()}.{fileExtension}";

            var content = File.ReadAllBytes("TestResources/test.jpg");
            using (var stream = new MemoryStream(content))
            {
                await _attachmentService.UploadAttachmentAsync(stream.ToArray(), AttachemntsUtils.GetMimeTypeByExtention(fileExtension), fileName);
            }

            await _profileService.UpdateUserPhotoAsync(new UpdateUserPhotoRequest(newProfile.UserId)
            {
                FileName = fileName
            });

            profile = await _profileService.GetProfileAsync(new ProfileRequest(newProfile.Id));
            Assert.NotNull(profile.PhotoUrl);
            byte[] imageBytes;
            using (var webClient = new WebClient())
            {
                imageBytes = webClient.DownloadData(profile.PhotoUrl);
            }
            Assert.NotNull(imageBytes);
            Assert.True(content.SequenceEqual(imageBytes));
        }


        [Fact]
        public async Task ShouldUpdateOtherUserPhotoAsync()
        {
            var model = new CreateProfileAuthorizedRequest(Guid.NewGuid().ToString(), Email)
            {
                Bio = "Test",
                FirstName = "Test",
                LastName = "Test",
                Gender = Gender.Male,
                Location = "Test",
                DateOfBirth = new DateTime()
            };
            var profile = await _profileService.CreateProfileAsync(model);
            var fileExtension = "jpg";
            var fileName = $"{Guid.NewGuid()}.{fileExtension}";

            var content = File.ReadAllBytes("TestResources/test.jpg");
            using (var stream = new MemoryStream(content))
            {
                await _attachmentService.UploadAttachmentAsync(stream.ToArray(), AttachemntsUtils.GetMimeTypeByExtention(fileExtension), fileName);
            }

            await _profileService.UpdateOtherUserPhotoAsync(new UpdateOtherUserPhotoRequest(profile.ProfileId, fileName));
            profile = await _profileService.GetProfileAsync(new ProfileRequest(profile.ProfileId));
            Assert.NotNull(profile.PhotoUrl);
            byte[] imageBytes;
            using (var webClient = new WebClient())
            {
                imageBytes = webClient.DownloadData(profile.PhotoUrl);
            }
            Assert.NotNull(imageBytes);
            Assert.True(content.SequenceEqual(imageBytes));
        }

        [Fact]
        public async Task ShouldDeleteUserPhotoAsync()
        {
            var model = new CreateProfileAuthorizedRequest(Guid.NewGuid().ToString(), Email)
            {
                Bio = "Test",
                FirstName = "Test",
                LastName = "Test",
                Gender = Gender.Male,
                Location = "Test",
                DateOfBirth = new DateTime()
            };
            var profile = await _profileService.CreateProfileAsync(model);
            var newProfile = await _profileService.GetProfileOrDefaultAsync(profile.ProfileId);
            var fileExtension = "jpg";
            var fileName = $"{Guid.NewGuid()}.{fileExtension}";

            var content = File.ReadAllBytes("TestResources/test.jpg");
            using (var stream = new MemoryStream(content))
            {
                await _attachmentService.UploadAttachmentAsync(stream.ToArray(), AttachemntsUtils.GetMimeTypeByExtention(fileExtension), fileName);
            }

            await _profileService.UpdateUserPhotoAsync(new UpdateUserPhotoRequest(newProfile.UserId)
            {
                FileName = fileName
            });

            var previousUrl = profile.PhotoUrl;
            await _profileService.DeleteUserPhotoAsync(new UserRequest(newProfile.UserId));
            profile = await _profileService.GetProfileAsync(new UserRequest(newProfile.UserId));
            Assert.NotNull(profile);
            Assert.Null(profile.PhotoUrl);
            var invalidOperation = false;
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadData(previousUrl);
                }
                catch (Exception)
                {
                    invalidOperation = true;
                }
            }
            Assert.True(invalidOperation);
        }

        [Fact]
        public async Task ShouldDeleteOtherUserPhotoAsync()
        {
            var model = new CreateProfileAuthorizedRequest(Guid.NewGuid().ToString(), Email)
            {
                Bio = "Test",
                FirstName = "Test",
                LastName = "Test",
                Gender = Gender.Male,
                Location = "Test",
                DateOfBirth = new DateTime()
            };
            var profile = await _profileService.CreateProfileAsync(model);
            var fileExtension = "jpg";
            var fileName = $"{Guid.NewGuid()}.{fileExtension}";

            var content = File.ReadAllBytes("TestResources/test.jpg");
            using (var stream = new MemoryStream(content))
            {
                await _attachmentService.UploadAttachmentAsync(stream.ToArray(), AttachemntsUtils.GetMimeTypeByExtention(fileExtension), fileName);
            }

            await _profileService.UpdateOtherUserPhotoAsync(new UpdateOtherUserPhotoRequest(profile.ProfileId, fileName));
            var previousUrl = profile.PhotoUrl;
            await _profileService.DeleteOtherUserPhotoAsync(new ProfileRequest(profile.ProfileId));
            var newProfile = await _profileService.GetProfileOrDefaultAsync(profile.ProfileId);
            Assert.NotNull(newProfile);
            Assert.Null(newProfile.PhotoName);
            var invalidOperation = false;
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadData(previousUrl);
                }
                catch (Exception)
                {
                    invalidOperation = true;
                }
            }
            Assert.True(invalidOperation);
        }

        [Fact]
        public async Task ShouldGetUserPhotoByBlobSasUriAsync()
        {
            var model = new CreateProfileAuthorizedRequest(Guid.NewGuid().ToString(), Email)
            {
                Bio = "Test",
                FirstName = "Test",
                LastName = "Test",
                Gender = Gender.Male,
                Location = "Test",
                DateOfBirth = new DateTime()
            };
            var profile = await _profileService.CreateProfileAsync(model);
            var newProfile = await _profileService.GetProfileOrDefaultAsync(profile.ProfileId);
            var fileExtension = "jpg";
            var fileName = $"{Guid.NewGuid()}.{fileExtension}";

            var content = File.ReadAllBytes("TestResources/test.jpg");
            using (var stream = new MemoryStream(content))
            {
                await _attachmentService.UploadAttachmentAsync(stream.ToArray(), AttachemntsUtils.GetMimeTypeByExtention(fileExtension), fileName);
            }

            await _profileService.UpdateUserPhotoAsync(new UpdateUserPhotoRequest(newProfile.UserId)
            {
                FileName = fileName
            });

            var blobSasUri = await _attachmentService.GetBlobSasUriAsync(new GetBlobSasUriRequest(_cloudStorageConfiguration.UserPhotoContainerName, fileName));
            byte[] imageBytes;
            using (var webClient = new WebClient())
            {
                imageBytes = webClient.DownloadData(blobSasUri);
            }
            Assert.NotNull(imageBytes);
            Assert.True(content.SequenceEqual(imageBytes));
        }

        [Fact]
        public async Task ShouldGetUserPhotoWithExpiredBlobSasUriAsync()
        {
            var model = new CreateProfileAuthorizedRequest(Guid.NewGuid().ToString(), Email)
            {
                Bio = "Test",
                FirstName = "Test",
                LastName = "Test",
                Gender = Gender.Male,
                Location = "Test",
                DateOfBirth = new DateTime()
            };
            var profile = await _profileService.CreateProfileAsync(model);
            var newProfile = await _profileService.GetProfileOrDefaultAsync(profile.ProfileId);
            var fileExtension = "jpg";
            var fileName = $"{Guid.NewGuid()}.{fileExtension}";

            var content = File.ReadAllBytes("TestResources/test.jpg");
            using (var stream = new MemoryStream(content))
            {
                await _attachmentService.UploadAttachmentAsync(stream.ToArray(), AttachemntsUtils.GetMimeTypeByExtention(fileExtension), fileName);
            }

            await _profileService.UpdateUserPhotoAsync(new UpdateUserPhotoRequest(newProfile.UserId)
            {
                FileName = fileName
            });

            var blobSasUri = await _attachmentService.GetBlobSasUriAsync(new GetBlobSasUriRequest(_cloudStorageConfiguration.UserPhotoContainerName, fileName));
            var expiredDateTime = "&se=" + DateTime.UtcNow.AddMinutes(-1).ToString("o");
            var index = blobSasUri.IndexOf("&se=");
            var nextIndex = blobSasUri.IndexOf('&', index + 1);
            blobSasUri = blobSasUri.Remove(index, nextIndex - index);
            var newBlobSasUri = blobSasUri.Insert(index, expiredDateTime);
            var invalidOperation = false;
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadData(newBlobSasUri);
                }
                catch (Exception ex)
                {
                    invalidOperation = true;
                }
            }
            Assert.True(invalidOperation);
        }
    }
}
