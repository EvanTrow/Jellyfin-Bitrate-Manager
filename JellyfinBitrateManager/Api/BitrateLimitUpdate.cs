using System;
using System.ComponentModel.DataAnnotations;

namespace JellyfinBitrateManager.Api
{
    /// <summary>
    /// A change to one user's internet streaming bitrate limit.
    /// </summary>
    public class BitrateLimitUpdate
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [Required]
        public Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the limit in bits per second. Zero means the server default applies.
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public int? RemoteClientBitrateLimit { get; set; }
    }
}
