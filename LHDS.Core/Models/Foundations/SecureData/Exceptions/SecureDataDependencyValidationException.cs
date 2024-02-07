// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace LHDS.Core.Models.Foundations.SecureData.Exceptions
{
    public class SecureDataDependencyValidationException : Exception
    {
        public SecureDataDependencyValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
