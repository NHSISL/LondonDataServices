// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Cryptographies.Exceptions
{
    public class NullSubscriberCredentialCryptographyException : Xeption
    {
        public NullSubscriberCredentialCryptographyException(string message)
            : base(message)
        { }
    }
}