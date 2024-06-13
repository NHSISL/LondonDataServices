// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.LibPostalClient.Exceptions
{
    public class InvalidLibPostalDependencyException : Xeption
    {
        public InvalidLibPostalDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
