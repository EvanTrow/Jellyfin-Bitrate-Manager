using System;
using System.Collections.Generic;
using JellyfinBitrateManager.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace JellyfinBitrateManager
{
    /// <summary>
    /// Bitrate Manager plugin entrypoint.
    /// </summary>
    public class BitrateManagerPlugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BitrateManagerPlugin"/> class.
        /// </summary>
        /// <param name="applicationPaths">Instance of the <see cref="IApplicationPaths"/> interface.</param>
        /// <param name="xmlSerializer">Instance of the <see cref="IXmlSerializer"/> interface.</param>
        public BitrateManagerPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
            : base(applicationPaths, xmlSerializer)
        {
        }

        /// <inheritdoc />
        public override string Name => "Bitrate Manager";

        /// <inheritdoc />
        public override Guid Id => Guid.Parse("0a10f5ce-80b8-4ae8-8b3a-f691de1cf00d");

        /// <inheritdoc />
        public IEnumerable<PluginPageInfo> GetPages()
        {
            return
            [
                new PluginPageInfo
                {
                    Name = "BitrateManager",
                    DisplayName = Name,
                    EmbeddedResourcePath = $"{GetType().Namespace}.Configuration.bitratePage.html",
                    EnableInMainMenu = true,
                    MenuIcon = "speed",
                },
            ];
        }
    }
}
