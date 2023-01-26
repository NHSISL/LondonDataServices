using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Downloads.Exceptions
{
    public class FailedDownloadServiceException : Xeption
    {
        public FailedDownloadServiceException(Exception innerException)
            : base(message: "Failed download service occurred, please contact support", innerException)
        { }
    }
}