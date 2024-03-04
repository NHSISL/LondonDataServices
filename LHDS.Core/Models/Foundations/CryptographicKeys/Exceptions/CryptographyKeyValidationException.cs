// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Cryptographies.Exceptions
{
    public class CryptographyKeyValidationException : Xeption
    {
        public CryptographyKeyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}