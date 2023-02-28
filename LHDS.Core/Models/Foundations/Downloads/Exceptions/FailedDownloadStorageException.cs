// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Downloads.Exceptions
{
    public class FailedDownloadStorageException : Xeption
    {
        public FailedDownloadStorageException(Exception innerException)
            : base(message: "Failed download storage error occurred, contact support.", innerException) { }
    }
}