// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Microsoft.Extensions.Configuration;

namespace LHDS.Core.Brokers.Storages.Blobs
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
            Configuration = configuration;
        }

        public string BlobContainerName => GetSettings("BlobStorage:blobContainerName", true);

        private IConfiguration Configuration { get; }

        private string GetSettings(string configurationKey, bool mandatory = true)
        {
            var value = Configuration[configurationKey];

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
