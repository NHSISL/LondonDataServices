// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections;
using Xeptions;

namespace LHDS.Core.Models.Clients.LibPostalClient.Exceptions
{
    public class InvalidLibPostalValidationException : Xeption
    {
        public InvalidLibPostalValidationException(string message, Xeption innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
