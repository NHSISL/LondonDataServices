// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Providers.FtpDownloads.Exceptions
{
    public class FailedToConnectSftpClientException : Xeption
    {
        public FailedToConnectSftpClientException()
            : base(message: "Failed to connect to SFTP client.") { }
    }
}
