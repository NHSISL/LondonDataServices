// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Providers.Downloads.FtpDownloads
{
    /// <summary>
    /// The FtpDownloadProviderSettings Class.
    /// </summary>
    public class FtpDownloadProviderSettings : IFtpDownloadProviderSettings
    {
        /// <inheritdoc/>
        public string FtpServer { get; set; }

        /// <inheritdoc/>
        public int FtpPort { get; set; } = 22;

        /// <inheritdoc/>
        public string FtpPassword { get; set; }

        /// <inheritdoc/>
        public string FtpUserName { get; set; }

        /// <inheritdoc/>
        public string TempFolder { get; set; }

        /// <inheritdoc/>
        public string FtpKey { get; set; }

        /// <inheritdoc/>
        public string FtpPassPhrase { get; set; }

        /// <inheritdoc/>
        public string FtpRootFolder { get; set; } = "/";

        public bool IncludeSubDirectories { get; set; } = true;
    }
}
