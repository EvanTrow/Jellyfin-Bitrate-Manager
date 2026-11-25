using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Jellyfin.Database.Implementations.Entities;
using MediaBrowser.Common.Api;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JellyfinBitrateManager.Api
{
    /// <summary>
    /// API controller for viewing and editing user bitrate limits. Administrators only.
    /// </summary>
    [ApiController]
    [Authorize(Policy = Policies.RequiresElevation)]
    [Route("BitrateManager")]
    [Produces(MediaTypeNames.Application.Json)]
    public class BitrateManagerController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IServerConfigurationManager _configurationManager;
        private readonly ILogger<BitrateManagerController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitrateManagerController"/> class.
        /// </summary>
        /// <param name="userManager">Instance of the <see cref="IUserManager"/> interface.</param>
        /// <param name="configurationManager">Instance of the <see cref="IServerConfigurationManager"/> interface.</param>
        /// <param name="logger">Instance of the <see cref="ILogger{BitrateManagerController}"/> interface.</param>
        public BitrateManagerController(
            IUserManager userManager,
            IServerConfigurationManager configurationManager,
            ILogger<BitrateManagerController> logger)
        {
            _userManager = userManager;
            _configurationManager = configurationManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets the bitrate limits of all users.
        /// </summary>
        /// <returns>The bitrate limits.</returns>
        [HttpGet("Users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<BitrateLimitsResponse> GetLimits()
        {
            var users = _userManager.GetUsers()
                .Select(user => _userManager.GetUserDto(user))
                .OrderBy(dto => dto.Name, StringComparer.CurrentCultureIgnoreCase)
                .Select(dto => new UserBitrateLimit
                {
                    UserId = dto.Id,
                    Name = dto.Name,
                    IsAdministrator = dto.Policy.IsAdministrator,
                    IsDisabled = dto.Policy.IsDisabled,
                    RemoteClientBitrateLimit = dto.Policy.RemoteClientBitrateLimit,
                })
                .ToArray();

            return new BitrateLimitsResponse
            {
                ServerRemoteClientBitrateLimit = _configurationManager.Configuration.RemoteClientBitrateLimit,
                Users = users,
            };
        }

        /// <summary>
        /// Updates the bitrate limits of the given users. Users that are not included are left unchanged.
        /// </summary>
        /// <param name="updates">The changes to apply.</param>
        /// <returns>No content on success.</returns>
        [HttpPost("Users")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateLimits([FromBody, Required] IReadOnlyList<BitrateLimitUpdate?> updates)
        {
            if (updates.Any(update => update is null))
            {
                return BadRequest("Updates must not contain null entries.");
            }

            // Resolve every user first so a bad id can't leave the batch half applied.
            var resolved = new List<(User User, int Limit)>(updates.Count);
            foreach (var update in updates)
            {
                var userId = update!.UserId!.Value;
                var user = userId == Guid.Empty ? null : _userManager.GetUserById(userId);
                if (user is null)
                {
                    return NotFound($"User {userId} was not found.");
                }

                resolved.Add((user, update.RemoteClientBitrateLimit!.Value));
            }

            foreach (var (user, limit) in resolved)
            {
                // Same path as Jellyfin's own user editor: change one field of the current policy and write it back.
                var policy = _userManager.GetUserDto(user).Policy;
                policy.RemoteClientBitrateLimit = limit;
                await _userManager.UpdatePolicyAsync(user.Id, policy).ConfigureAwait(false);
                _logger.LogInformation("Set internet streaming bitrate limit of {UserName} to {Limit} bps", user.Username, limit);
            }

            return NoContent();
        }
    }
}
