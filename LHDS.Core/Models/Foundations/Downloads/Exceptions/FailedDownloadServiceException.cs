// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Downloads.Exceptions
{
    public class FailedDownloadServiceException : Xeption
    {
        public FailedDownloadServiceException(Exception innerException)
            : base(message: "Failed download service occurred, please contact support", innerException)
        { }
    }
}