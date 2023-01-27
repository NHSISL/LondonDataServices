using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Downloads.Exceptions
{
    public class NotFoundDownloadException : Xeption
    {
        public NotFoundDownloadException(Guid downloadId)
            : base(message: $"Couldn't find download with downloadId: {downloadId}.")
        { }
    }
}