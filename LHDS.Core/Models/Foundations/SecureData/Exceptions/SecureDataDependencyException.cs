// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SecureData.Exceptions
{
    public class SecureDataDependencyException : Xeption
    {
        public SecureDataDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
