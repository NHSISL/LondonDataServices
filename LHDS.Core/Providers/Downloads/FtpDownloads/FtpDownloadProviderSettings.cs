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
        public string FtpServer { get; set; } = string.Empty;
        public int FtpPort { get; set; } = 22;
        public string FtpUserName { get; set; } = string.Empty;
        public string FtpPassword { get; set; } = string.Empty;
        public string FtpPassPhrase { get; set; } = string.Empty;
        public string FtpPrivateKey { get; set; } = string.Empty;
        public string FtpRootFolder { get; set; } = "/";
        public bool IncludeSubDirectories { get; set; } = true;
    }
}
