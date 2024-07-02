// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Cryptographies.Exceptions
{
    public class InvalidArgumentCryptographyException : Xeption
    {
        public InvalidArgumentCryptographyException(string message)
            : base(message)
        { }
    }
}