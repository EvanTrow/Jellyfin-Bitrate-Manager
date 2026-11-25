using System.Collections.Generic;

namespace JellyfinBitrateManager.Api
{
    /// <summary>
    /// The bitrate limits of all users.
    /// </summary>
    public class BitrateLimitsResponse
    {
        /// <summary>
        /// Gets or sets the server-wide limit in bits per second. Zero means unlimited.
        /// </summary>
        public int ServerRemoteClientBitrateLimit { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        public IReadOnlyList<UserBitrateLimit> Users { get; set; } = [];
    }
}
