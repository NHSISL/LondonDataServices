// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Downloads.Exceptions
{
    public class InvalidDownloadReferenceException : Xeption
    {
        public InvalidDownloadReferenceException(Exception innerException)
            : base(message: "Invalid download reference error occurred.", innerException) { }
    }
}