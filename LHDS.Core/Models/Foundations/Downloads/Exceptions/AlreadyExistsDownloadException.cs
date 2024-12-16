// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Downloads.Exceptions
{
    public class AlreadyExistsDownloadException : Xeption
    {
        public AlreadyExistsDownloadException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}