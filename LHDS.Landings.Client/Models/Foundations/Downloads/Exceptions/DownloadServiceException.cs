// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.Downloads.Exceptions
{
    public class DownloadServiceException : Xeption
    {
        public DownloadServiceException(Exception innerException)
            : base(message: "Download service error occurred, contact support.", innerException)
        { }
    }
}