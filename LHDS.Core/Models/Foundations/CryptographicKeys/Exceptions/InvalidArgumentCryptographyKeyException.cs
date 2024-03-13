// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions
{
    public class InvalidArgumentCryptographyKeyException : Xeption
    {
        public InvalidArgumentCryptographyKeyException(string message)
            : base(message)
        { }
    }
}
