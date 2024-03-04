// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions
{
    public class NullPublicKeyCommentCryptographyKeyException : Xeption
    {
        public NullPublicKeyCommentCryptographyKeyException(string message)
            : base(message)
        { }
    }
}