// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.FileNameValidation.Exceptions
{
    public class FailedFileNameValidationServiceException : Xeption
    {
        public FailedFileNameValidationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}