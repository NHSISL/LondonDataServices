// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Providers.Downloads.FtpDownloads
{
    /// <summary>
    /// The FtpDownloadProviderSettings Interface.
    /// </summary>
    public interface IFtpDownloadProviderSettings
    {
        /// <summary>
        /// Gets the ftp server user name.
        /// </summary>
        string FtpUserName { get; }

        /// <summary>
        /// Gets the ftp key.
        /// </summary>
        string FtpPrivateKey { get; }

        /// <summary>
        /// Gets the ftp pass phrase.
        /// </summary>
        string FtpPassPhrase { get; }

        /// <summary>
        /// Gets the ftp server host name.
        /// </summary>
        string FtpServer { get; }

        /// <summary>
        /// Gets the ftp service port number.
        /// </summary>
        int FtpPort { get; }

        /// <summary>
        /// Gets the ftp server password.
        /// </summary>
        string FtpPassword { get; }

        /// <summary>
        /// Gets the root folder for the FTP server.
        /// </summary>
        string FtpRootFolder { get; }

        /// <summary>
        /// Gets the items from sub folders if true.
        /// </summary>
        bool IncludeSubDirectories { get; }
    }
}
