// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SecureData.Exceptions
{
    public class InvalidSecureDataException : Xeption
    {
        public InvalidSecureDataException(string message)
            : base(message)
        { }
    }
}