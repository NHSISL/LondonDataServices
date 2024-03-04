// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions
{
    public class NullCryptographyTypeCryptographyKeyException : Xeption
    {
        public NullCryptographyTypeCryptographyKeyException(string message)
            : base(message)
        { }
    }
}