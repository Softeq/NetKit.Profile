using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Softeq.NetKit.Profile.Domain.Models.Profile;
using Softeq.NetKit.ProfileService.Exceptions;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Queries;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Request;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Response;
using Softeq.NetKit.ProfileService.TransportModels.Shared;
using Softeq.QueryUtils;
using Xunit;

namespace Softeq.NetKit.Profile.Test.Unit
{
    public class ProfileServiceTests : ProfileServiceTestsBase
    {
        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldThrowIfProfileAlreadyExist()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                Bio = "bio",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location",
            };

            var profileRequest = new CreateProfileAuthorizedRequest(userProfile.UserId, userProfile.Email)
            {
                Bio = userProfile.Bio,
                DateOfBirth = userProfile.DateOfBirth,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Gender = userProfile.Gender,
                Location = userProfile.Location
            };

            var userProfiles = new List<UserProfile>
            {
                userProfile
            };

            var mock = userProfiles.AsQueryable().BuildMock();
            
            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            // Act
            async Task Act() => await _profileService.CreateProfileAsync(profileRequest);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(Act);

            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldCreateUserProfile()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location"
            };

            var profileRequest = new CreateProfileAuthorizedRequest(userProfile.UserId, userProfile.Email)
            {
                Bio = userProfile.Bio,
                DateOfBirth = userProfile.DateOfBirth,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Gender = userProfile.Gender,
                Location = userProfile.Location
            };
            
            var mock = new List<UserProfile>().AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Add(It.IsAny<UserProfile>()))
                .Returns(userProfile)
                .Verifiable();

            // Act
            var profileResponse = await _profileService.CreateProfileAsync(profileRequest);

            // Assert
            Assert.True(profileResponse.Email == userProfile.Email);
            Assert.True(profileResponse.ProfileId == userProfile.Id);
            Assert.True(profileResponse.FirstName == userProfile.FirstName);
            Assert.True(profileResponse.LastName == userProfile.LastName);
            Assert.True(profileResponse.Gender == userProfile.Gender);
            Assert.True(profileResponse.DateOfBirth == userProfile.DateOfBirth);

            VerifyMocks();
        }


        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldThrowIfProfileNotExist()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location"
            };

            var profileRequest = new UpdateProfileRequest(userProfile.UserId)
            {
                Bio = userProfile.Bio,
                DateOfBirth = userProfile.DateOfBirth,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Gender = userProfile.Gender,
                Location = userProfile.Location
            };

            var mock = new List<UserProfile>().AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            // Act
            async Task Act() => await _profileService.UpdateProfileAsync(profileRequest);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Act);

            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldUpdateProfile()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location"
            };

            var profileRequest = new UpdateProfileRequest(userProfile.UserId)
            {
                Bio = $"{userProfile.Bio}updated",
                DateOfBirth = userProfile.DateOfBirth.Value.AddDays(1),
                FirstName = $"{userProfile.FirstName}updated",
                LastName = $"{userProfile.LastName}updated",
                Gender = Gender.Female,
                Location = $"{userProfile.Location}updated"
            };

            var userProfiles = new List<UserProfile>
            {
                userProfile
            };

            var mock = userProfiles.AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Update(It.IsAny<UserProfile>()))
                .Verifiable();

            var updatedProfile = await _profileService.UpdateProfileAsync(profileRequest);

            // Assert
            Assert.True(updatedProfile.ProfileId == userProfile.Id);
            Assert.True(updatedProfile.FirstName == profileRequest.FirstName);
            Assert.True(updatedProfile.LastName == profileRequest.LastName);
            Assert.True(updatedProfile.Gender == profileRequest.Gender);
            Assert.True(updatedProfile.DateOfBirth == profileRequest.DateOfBirth);

            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldThrowUpdateOtherProfileIfProfileNotFound()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location"
            };

            var profileRequest = new UpdateOtherProfileRequest
            {
                Bio = $"{userProfile.Bio}updated",
                DateOfBirth = userProfile.DateOfBirth.Value.AddDays(1),
                FirstName = $"{userProfile.FirstName}updated",
                LastName = $"{userProfile.LastName}updated",
                Gender = Gender.Female,
                Location = $"{userProfile.Location}updated",
                RequestedProfileId = userProfile.Id
            };

            var mock = new List<UserProfile>().AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            // Act
            async Task Act() => await _profileService.UpdateOtherProfileAsync(profileRequest);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Act);

            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldUpdateOtherProfile()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location"
            };

            var profileRequest = new UpdateOtherProfileRequest
            {
                Bio = $"{userProfile.Bio}updated",
                DateOfBirth = userProfile.DateOfBirth.Value.AddDays(1),
                FirstName = $"{userProfile.FirstName}updated",
                LastName = $"{userProfile.LastName}updated",
                Gender = Gender.Female,
                Location = $"{userProfile.Location}updated",
                RequestedProfileId = userProfile.Id
            };

            var userProfiles = new List<UserProfile>
            {
                userProfile
            };

            var mock = userProfiles.AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Update(It.IsAny<UserProfile>()))
                .Verifiable();

            // Act
            var updatedProfile = await _profileService.UpdateOtherProfileAsync(profileRequest);

            // Assert
            Assert.True(updatedProfile.ProfileId == userProfile.Id);
            Assert.True(updatedProfile.FirstName == profileRequest.FirstName);
            Assert.True(updatedProfile.LastName == profileRequest.LastName);
            Assert.True(updatedProfile.Gender == profileRequest.Gender);
            Assert.True(updatedProfile.DateOfBirth == profileRequest.DateOfBirth);

            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldGetProfileOrDefault()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location"
            };

            var userProfiles = new List<UserProfile>
            {
                userProfile
            };

            var mock = userProfiles.AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            // Act
            var updatedProfile = await _profileService.GetProfileOrDefaultAsync(userProfile.Id);

            // Assert
            Assert.True(updatedProfile.Id == userProfile.Id);
            Assert.True(updatedProfile.FirstName == userProfile.FirstName);
            Assert.True(updatedProfile.LastName == userProfile.LastName);
            Assert.True(updatedProfile.Gender == userProfile.Gender);
            Assert.True(updatedProfile.DateOfBirth == userProfile.DateOfBirth);

            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldThrowGetProfileByProfileRequstIfProfileNotFound()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location"
            };

            var profileRequest = new ProfileRequest(userProfile.Id);

            var mock = new List<UserProfile>().AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            // Act
            async Task Act() => await _profileService.GetProfileAsync(profileRequest);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Act);

            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldGetProfileByProfileRequst()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location"
            };

            var userProfiles = new List<UserProfile>
            {
                userProfile
            };

            var mock = userProfiles.AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            var profileRequest = new ProfileRequest(userProfile.Id);

            // Act
            var receivedProfile = await _profileService.GetProfileAsync(profileRequest);

            // Assert
            Assert.True(receivedProfile.ProfileId == userProfile.Id);
            Assert.True(receivedProfile.FirstName == userProfile.FirstName);
            Assert.True(receivedProfile.LastName == userProfile.LastName);
            Assert.True(receivedProfile.Gender == userProfile.Gender);
            Assert.True(receivedProfile.DateOfBirth == userProfile.DateOfBirth);

            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldThrowGetProfileIfProfileNotFound()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location"
            };

            var userRequest = new UserRequest(userProfile.Id.ToString());

            var mock = new List<UserProfile>().AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            // Act
            async Task Act() => await _profileService.GetProfileAsync(userRequest);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Act);

            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldGetProfile()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location"
            };

            var userProfiles = new List<UserProfile>
            {
                userProfile
            };

            var mock = userProfiles.AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            var userRequest = new UserRequest(userProfile.Id.ToString());

            // Act
            var receivedProfile = await _profileService.GetProfileAsync(userRequest);

            // Assert
            Assert.True(receivedProfile.ProfileId == userProfile.Id);
            Assert.True(receivedProfile.FirstName == userProfile.FirstName);
            Assert.True(receivedProfile.LastName == userProfile.LastName);
            Assert.True(receivedProfile.Gender == userProfile.Gender);
            Assert.True(receivedProfile.DateOfBirth == userProfile.DateOfBirth);

            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldThrowDeleteUserPhotoIfProfileNotFound()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location"
            };

            var userRequest = new UserRequest(userProfile.Id.ToString());

            var mock = new List<UserProfile>().AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            // Act
            async Task Act() => await _profileService.DeleteUserPhotoAsync(userRequest);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Act);

            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldDeleteUserPhoto()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location",
                PhotoName = "photoName"
            };

            var userProfiles = new List<UserProfile>
            {
                userProfile
            };

            var mock = userProfiles.AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            _contentStorageMock.Setup(x => x.DeleteContentAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Update(It.IsAny<UserProfile>()))
                .Verifiable();

            var userRequest = new UserRequest(userProfile.Id.ToString());

            // Act
            await _profileService.DeleteUserPhotoAsync(userRequest);

            // Assert
            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldThrowDeleteOtherUserPhotoIfProfileNotFound()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location"
            };

            var profileRequest = new ProfileRequest(userProfile.Id);

            var mock = new List<UserProfile>().AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            // Act
            async Task Act() => await _profileService.DeleteOtherUserPhotoAsync(profileRequest);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Act);

            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldDeleteOtherUserPhoto()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location",
                PhotoName = "photoName"
            };

            var userProfiles = new List<UserProfile>
            {
                userProfile
            };

            var mock = userProfiles.AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            _contentStorageMock.Setup(x => x.DeleteContentAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Update(It.IsAny<UserProfile>()))
                .Verifiable();

            var profileRequest = new ProfileRequest(userProfile.Id);

            // Act
            await _profileService.DeleteOtherUserPhotoAsync(profileRequest);

            // Assert
            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldThrowUpdateUserPhotoIfProfileNotFound()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location"
            };

            var updateOtherUserPhotoRequest = new UpdateUserPhotoRequest(userProfile.Id.ToString())
            {
                FileName = "fileName"
            };

            var mock = new List<UserProfile>().AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            // Act
            async Task Act() => await _profileService.UpdateUserPhotoAsync(updateOtherUserPhotoRequest);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Act);

            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldUpdateUserPhoto()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location",
                PhotoName = "photoName"
            };

            var updateOtherUserPhotoRequest = new UpdateUserPhotoRequest(userProfile.Id.ToString())
            {
                FileName = "fileName"
            };

            var userProfiles = new List<UserProfile>
            {
                userProfile
            };

            var mock = userProfiles.AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            _contentStorageMock.Setup(x => x.CopyBlobAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Uri("http://userprofile.test"))
                .Verifiable();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Update(It.IsAny<UserProfile>()))
                .Verifiable();

            // Act
            await ShouldDeleteUserPhoto();
            await _profileService.UpdateUserPhotoAsync(updateOtherUserPhotoRequest);

            // Assert
            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldThrowUpdateOtherUserPhotoIfProfileNotFound()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location"
            };

            var updateOtherUserPhotoRequest = new UpdateOtherUserPhotoRequest(userProfile.Id, "fileName");

            var mock = new List<UserProfile>().AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            // Act
            async Task Act() => await _profileService.UpdateOtherUserPhotoAsync(updateOtherUserPhotoRequest);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Act);

            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldUpdateOtherUserPhoto()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "123",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location",
                PhotoName = "photoName"
            };

            var updateOtherUserPhotoRequest = new UpdateOtherUserPhotoRequest(userProfile.Id, "fileName");

            var userProfiles = new List<UserProfile>
            {
                userProfile
            };

            var mock = userProfiles.AsQueryable().BuildMock();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Query(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(mock.Object)
                .Verifiable();

            _contentStorageMock.Setup(x => x.CopyBlobAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Uri("http://userprofile.test"))
                .Verifiable();

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.Update(It.IsAny<UserProfile>()))
                .Verifiable();

            // Act
            await ShouldDeleteUserPhoto();
            await _profileService.UpdateOtherUserPhotoAsync(updateOtherUserPhotoRequest);

            // Assert
            VerifyMocks();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ShouldGetProfiles()
        {
            // Arrange
            var userProfile1 = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user1@test.com",
                UserId = "1",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName",
                LastName = "lastName",
                Gender = Gender.Male,
                Location = "location",
                PhotoName = "photoName"
            };

            var userProfile2 = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user2@test.com",
                UserId = "2",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName2",
                LastName = "lastName2",
                Gender = Gender.Male,
                Location = "location2",
                PhotoName = "photoName2"
            };

            var userProfile3 = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "user3@test.com",
                UserId = "3",
                DateOfBirth = DateTime.Parse("05-05-2019"),
                FirstName = "firstName3",
                LastName = "lastName3",
                Gender = Gender.Male,
                Location = "location3",
                PhotoName = "photoName3"
            };

            var getProfilesRequest = new GetProfilesQuery
            {
                Page = 1,
                PageSize = 2,
                Filters = new List<Filter>()
            };

            var userProfiles = new List<UserProfile>
            {
                userProfile1,
                userProfile2,
                userProfile3
            };

            var mock = userProfiles.AsQueryable().BuildMock();

            _contentStorageMock.Setup(x => x.GetBlobSasUriAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(string.Empty));

            _unitOfWorkMock.Setup(x => x.UserProfileRepository.GetAll())
                .Returns(mock.Object)
                .Verifiable();
            
            // Act
            var result = await _profileService.GetProfilesAsync(getProfilesRequest);

            // Assert
            Assert.True(result.PageNumber == 1);
            Assert.True(result.PageSize == 2);
            Assert.True(result.TotalNumberOfPages == 2);
            Assert.True(result.Results.Count() == 2);

            VerifyMocks();
        }
    }
}
