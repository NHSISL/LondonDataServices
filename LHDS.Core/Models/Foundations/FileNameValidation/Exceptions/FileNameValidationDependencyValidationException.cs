// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.FileNameValidations.Exceptions
{
    public class FileNameValidationDependencyValidationException : Xeption
    {
        public FileNameValidationDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}