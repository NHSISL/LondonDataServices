// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Microsoft.Extensions.Configuration;

namespace LHDS.Landings.Client.Providers.Downloads.FtpDownloads
{
    /// <summary>
    /// The FtpDownloadProviderSettings Class.
    /// </summary>
    public class FtpDownloadProviderSettings : IFtpDownloadProviderSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public FtpDownloadProviderSettings(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <inheritdoc/>
        public string FtpServer => this.GetSettings("ftpDownload:ftpServer");

        /// <inheritdoc/>
        public int FtpPort
        {
            get
            {
                try
                {
                    return int.Parse(this.GetSettings("ftpDownload:ftpPort"));
                }
                catch
                {
                    return 22;
                }
            }
        }

        /// <inheritdoc/>
        public string FtpPassword => this.GetSettings("ftpDownload:ftpPassword", false);

        /// <inheritdoc/>
        public string FtpUserName => this.GetSettings("ftpDownload:ftpUserName");

        /// <inheritdoc/>
        public string TempFolder => this.GetSettings("ftpDownload:tempFolder");

        /// <inheritdoc/>
        public string FtpKey => this.GetSettings("ftpDownload:ftpKey", false);

        /// <inheritdoc/>
        public string FtpPassPhrase => this.GetSettings("ftpDownload:ftpPassPhrase", false);

        /// <inheritdoc/>
        public string FtpRootFolder => this.GetSettings("ftpDownload:ftpRootFolder", false) ?? "/";

        public bool IncludeSubDirectories =>
            Convert.ToBoolean(this.GetSettings("ftpDownload:includeSubDirectories", false) ?? "true");

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
