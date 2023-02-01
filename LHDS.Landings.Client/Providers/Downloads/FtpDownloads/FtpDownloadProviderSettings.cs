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
        public string FtpServer => this.GetSettings("FtpDownload:FtpServer");

        /// <inheritdoc/>
        public int FtpPort
        {
            get
            {
                try
                {
                    return int.Parse(this.GetSettings("FtpDownload:FtpPort"));
                }
                catch
                {
                    return 22;
                }
            }
        }

        /// <inheritdoc/>
        public string FtpPassword => this.GetSettings("FtpDownload:FtpPassword", false);

        /// <inheritdoc/>
        public string FtpUserName => this.GetSettings("FtpDownload:FtpUserName");

        /// <inheritdoc/>
        public string TempFolder => this.GetSettings("FtpDownload:TempFolder");

        /// <inheritdoc/>
        public bool TestMode
        {
            get
            {
                return Convert.ToBoolean(this.GetSettings("FtpDownload:TestMode"));
            }
        }

        /// <inheritdoc/>
        public string FtpKey => this.GetSettings("FtpDownload:FtpKey", false);

        /// <inheritdoc/>
        public string FtpPassPhrase => this.GetSettings("FtpDownload:FtpPassPhrase", false);

        /// <inheritdoc/>
        public string FtpRootFolder => this.GetSettings("FtpDownload:FtpRootFolder", false) ?? "/";

        public bool IncludeSubDirectories =>
            Convert.ToBoolean(this.GetSettings("FtpDownload:IncludeSubDirectories", false) ?? "true");

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
