// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SecureData.Exceptions
{
    public class SecureDataDependencyValidationException : Xeption
    {
        public SecureDataDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
