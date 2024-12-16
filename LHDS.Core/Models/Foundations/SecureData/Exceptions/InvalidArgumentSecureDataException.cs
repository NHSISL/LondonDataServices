// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SecureData.Exceptions
{
    public class InvalidArgumentSecureDataException : Xeption
    {
        public InvalidArgumentSecureDataException(string message)
            : base(message)
        { }
    }
}