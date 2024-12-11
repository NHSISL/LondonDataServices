// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Cryptographies.Exceptions
{
    public class CryptographyDependencyValidationException : Xeption
    {
        public CryptographyDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}