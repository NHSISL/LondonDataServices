using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Downloads.Exceptions
{
    public class DownloadServiceException : Xeption
    {
        public DownloadServiceException(Exception innerException)
            : base(message: "Download service error occurred, contact support.", innerException)
        { }
    }
}