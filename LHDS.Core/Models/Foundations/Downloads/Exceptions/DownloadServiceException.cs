// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Downloads.Exceptions
{
    public class DownloadServiceException : Xeption
    {
        public DownloadServiceException(string message, Exception? innerException)
            : base(message, innerException) 
        { }
    }
}