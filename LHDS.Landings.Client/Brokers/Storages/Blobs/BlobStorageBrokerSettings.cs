// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Microsoft.Extensions.Configuration;

namespace LHDS.Landings.Client.Providers.Downloads.FtpDownloads
{
    /// <summary>
    /// The Settings Class.
    /// </summary>
    public class BlobStorageBrokerSettings : IBlobStorageBrokerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public BlobStorageBrokerSettings(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <inheritdoc/>
        public string AzureBlobStoreUri => this.GetSettings("blobStorage:azureBlobStoreUri", true);

        public string BlobContainerName => this.GetSettings("blobStorage:blobContainerName", true);

        private IConfiguration Configuration { get; }

        private string GetSettings(string configurationKey, bool mandatory = true)
        {
            var value = this.Configuration[configurationKey];

            if (string.IsNullOrEmpty(value))
            {
                if (mandatory)
                {
                    throw new Exception($"Configuration value {configurationKey} does not exist");
                }
            }

            return value;
        }
    }
}
