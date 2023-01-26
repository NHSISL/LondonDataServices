using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Downloads.Exceptions
{
    public class FailedDownloadStorageException : Xeption
    {
        public FailedDownloadStorageException(Exception innerException)
            : base(message: "Failed download storage error occurred, contact support.", innerException)
        { }
    }
}