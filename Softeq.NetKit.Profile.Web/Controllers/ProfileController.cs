// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EnsureThat;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Softeq.NetKit.Profile.Web.ExceptionHandling;
using Softeq.NetKit.ProfileService.Abstract;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Queries;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Request;
using Softeq.NetKit.ProfileService.TransportModels.Profile.Response;
using Softeq.NetKit.ProfileService.TransportModels.Shared;
using Softeq.NetKit.ProfileService.Utility.ErrorHandling;
using Softeq.QueryUtils;
using Softeq.Serilog.Extension;

namespace Softeq.NetKit.Profile.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/me/profile")]
    [ApiVersion("1.0")]
    [Authorize]
    [ProducesResponseType(typeof(List<ErrorDto>), 400)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    [ProducesResponseType(typeof(ErrorDto), 500)]
    public sealed class ProfileController : Controller
    {
        private readonly IAttachmentService _attachmentService;
        private readonly IProfileService _profileService;
        private readonly ILogger _logger;

        public ProfileController(IProfileService profileService, IAttachmentService attachmentService, ILogger logger)
        {
            _attachmentService = attachmentService;
            _profileService = Ensure.Any.IsNotNull(profileService, nameof(profileService));
            _logger = Ensure.Any.IsNotNull(logger, nameof(logger));
        }

        /// <summary>
        ///      Get all user profiles
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PagedResults<ProfileResponse>), 200)]
        [Route("/api/profiles")]
        public async Task<IActionResult> GetProfilesAsync([FromBody] GetProfilesQuery query)
        {
            var res = await _profileService.GetProfilesAsync(query);
            return Ok(res);
        }

        /// <summary>
        ///      Get current user profile
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ProfileResponse), 200)]
        public async Task<IActionResult> GetMyProfileAsync()
        {
            var userId = GetClaimValue(JwtClaimTypes.Subject);
            var profile = await _profileService.GetProfileAsync(new UserRequest(userId));
            return Json(profile);
        }

        /// <summary>
        ///     Get user profile
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("/api/profile/{profileId:guid}")]
        [ProducesResponseType(typeof(ProfileResponse), 200)]
        public async Task<IActionResult> GetOtherUserProfileAsync(Guid profileId)
        {
            var profile = await _profileService.GetProfileAsync(new ProfileRequest(profileId));
            return Json(profile);
        }

        /// <summary>
        ///     Create current user profile
        /// </summary>
        /// <param name="model">User profile</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProfileResponse), 200)]
        public async Task<IActionResult> CreateProfileAsync([FromBody] CreateProfileRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ToErrorModel());
            }

            string userId = GetClaimValue(JwtClaimTypes.Subject);

            _logger.Event(PropertyNames.EventId).With.Message("Create user. UserId: {userId}", userId).AsInformation();
            var profile = await _profileService.CreateProfileAsync(new CreateProfileAuthorizedRequest(userId, GetClaimValue(JwtClaimTypes.Email))
            {
                Bio = model.Bio,
                DateOfBirth = model.DateOfBirth,
                FirstName = model.FirstName,
                Gender = model.Gender,
                LastName = model.LastName,
                Location = model.Location
            });
            return Ok(profile);
        }

        /// <summary>
        ///     Update current user profile
        /// </summary>
        /// <param name="model">User profile</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ProfileResponse), 200)]
        public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdateProfileRequest model)
        {
            model.UserId = GetClaimValue(JwtClaimTypes.Subject);
            if (ModelState.IsValid)
            {
                var profile = await _profileService.UpdateProfileAsync(model);
                return Ok(profile);
            }
            return BadRequest(ModelState.ToErrorModel());
        }

        /// <summary>
        ///     Update other user profile
        /// </summary>
        /// <param name="model">User profile</param>
        /// /// <param name="profileId">Requested profile id</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("/api/profile/{profileId:guid}")]
        [ProducesResponseType(typeof(ProfileResponse), 200)]
        public async Task<IActionResult> UpdateOtherProfileAsync(Guid profileId, [FromBody] UpdateOtherProfileRequest model)
        {
            model.RequestedProfileId = profileId;
            if (ModelState.IsValid)
            {
                var profile = await _profileService.UpdateOtherProfileAsync(model);
                return Ok(profile);
            }
            return BadRequest(ModelState.ToErrorModel());
        }

        /// <summary>
        ///     Get sas user token
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(ContainerAccessTokenResponse), 200)]
        [HttpGet]
        [Route("/api/container/key")]
        public async Task<IActionResult> GetSasTokenAsync()
        {
            var token = await _attachmentService.GetContainerAccessKeyAsync();
            return Ok(token);
        }

        /// <summary>
        ///     Update current user profile photo
        /// </summary>
        /// <param name="model">Upload file model</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(void), 200)]
        [HttpPut]
        [Route("photo")]
        public async Task<IActionResult> UploadPhotoAsync([FromBody] UpdateUserPhotoRequest model)
        {
            model.UserId = GetClaimValue(JwtClaimTypes.Subject);
            await _profileService.UpdateUserPhotoAsync(model);
            return Ok();
        }

        /// <summary>
        ///     Update current user profile photo
        /// </summary>
        /// <param name="model">Upload file model</param>
        /// <param name="profileId">Requested profile id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(void), 200)]
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("/api/profile/{profileId:guid}/photo")]
        public async Task<IActionResult> UploadOtherProfilePhotoAsync(Guid profileId, [FromBody] UpdateOtherUserPhotoRequest model)
        {
            model.RequestedProfileId = profileId;
            await _profileService.UpdateOtherUserPhotoAsync(model);
            return Ok();
        }

        /// <summary>
        ///     Delete user profile photo
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(void), 200)]
        [HttpDelete]
        [Route("photo")]
        public async Task<IActionResult> DeleteUserPhotoAsync()
        {
            var userId = GetClaimValue(JwtClaimTypes.Subject);
            await _profileService.DeleteUserPhotoAsync(new UserRequest(userId));
            return Ok();
        }

        /// <summary>
        ///    Delete user profile photo
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(void), 200)]
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("/api/profile/{profileId:guid}/photo")]
        public async Task<IActionResult> DeleteOtherUserPhotoAsync(Guid profileId)
        {
            await _profileService.DeleteOtherUserPhotoAsync(new ProfileRequest(profileId));
            return Ok();
        }

        private string GetClaimValue(string claim)
        {
            return this.User.FindFirstValue(claim);
        }
    }
}