using System;

namespace JellyfinBitrateManager.Api
{
    /// <summary>
    /// A user and their internet streaming bitrate limit.
    /// </summary>
    public class UserBitrateLimit
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the user is an administrator.
        /// </summary>
        public bool IsAdministrator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets the limit in bits per second. Zero means the server default applies.
        /// </summary>
        public int RemoteClientBitrateLimit { get; set; }
    }
}
