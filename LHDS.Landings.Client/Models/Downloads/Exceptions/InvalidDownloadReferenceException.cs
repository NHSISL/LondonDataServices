using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Downloads.Exceptions
{
    public class InvalidDownloadReferenceException : Xeption
    {
        public InvalidDownloadReferenceException(Exception innerException)
            : base(message: "Invalid download reference error occurred.", innerException) { }
    }
}