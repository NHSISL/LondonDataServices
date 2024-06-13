// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SecureData.Exceptions
{
    public class SecureDataValidationException : Xeption
    {
        public SecureDataValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}