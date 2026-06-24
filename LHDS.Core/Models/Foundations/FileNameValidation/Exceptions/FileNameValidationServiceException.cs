// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.FileNameValidation.Exceptions
{
    internal class FileNameValidationServiceException : Xeption
    {
        public FileNameValidationServiceException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
