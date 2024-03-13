// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions
{
    public class NullBrokerCryptographyKeyException : Xeption
    {
        public NullBrokerCryptographyKeyException(string message)
            : base(message)
        { }
    }
}