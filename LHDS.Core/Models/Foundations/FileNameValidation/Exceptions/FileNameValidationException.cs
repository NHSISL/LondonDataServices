// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.FileNameValidation.Exceptions
{
    internal class FileNameValidationException : Xeption
    {
        public FileNameValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
