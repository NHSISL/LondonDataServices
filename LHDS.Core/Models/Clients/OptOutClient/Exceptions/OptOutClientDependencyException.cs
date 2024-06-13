// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.OptOutClient.Exceptions
{
    public class OptOutClientDependencyException : Xeption
    {
        public OptOutClientDependencyException(Xeption innerException)
            : base(message: "Opt out client dependency error occurred, please contact support.",
                  innerException)
        { }
    }
}
