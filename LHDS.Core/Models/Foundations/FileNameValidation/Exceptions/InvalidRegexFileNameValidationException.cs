// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.FileNameValidations.Exceptions
{
    public class InvalidRegexFileNameValidationException : Xeption
    {
        public InvalidRegexFileNameValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}