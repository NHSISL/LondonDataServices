// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions
{
    public class CryptographyKeyDependencyValidationException : Xeption
    {
        public CryptographyKeyDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}