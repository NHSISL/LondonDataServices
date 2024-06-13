// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.DecryptClient.Exceptions
{
    public class DecryptionClientDependencyException : Xeption
    {
        public DecryptionClientDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
