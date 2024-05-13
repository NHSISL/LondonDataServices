// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.Downloads.Exceptions
{
    public class FailedDownloadProcessingServiceException : Xeption
    {
        public FailedDownloadProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
