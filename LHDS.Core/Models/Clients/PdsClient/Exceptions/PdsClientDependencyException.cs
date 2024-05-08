// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.PdsClient.Exceptions
{
    public class PdsClientDependencyException : Xeption
    {
        public PdsClientDependencyException(Xeption innerException)
            : base(message: "PDS client dependency error occurred, please contact support.",
                  innerException)
        { }
    }
}
