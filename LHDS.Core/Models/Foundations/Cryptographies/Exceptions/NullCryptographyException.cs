// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Cryptographies.Exceptions
{
    public class NullCryptographyException : Xeption
    {
        public NullCryptographyException(string message)
            : base(message)
        { }
    }
}