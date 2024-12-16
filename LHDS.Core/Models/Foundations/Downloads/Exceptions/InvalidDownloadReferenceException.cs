// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Downloads.Exceptions
{
    public class InvalidDownloadReferenceException : Xeption
    {
        public InvalidDownloadReferenceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}