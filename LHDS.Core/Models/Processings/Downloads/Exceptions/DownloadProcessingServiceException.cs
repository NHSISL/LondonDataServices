// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Downloads.Exceptions
{
    public class DownloadProcessingServiceException : Xeption
    {
        public DownloadProcessingServiceException(string message, Xeption? innerException)
          : base(message, innerException)
        { }
    }
}
