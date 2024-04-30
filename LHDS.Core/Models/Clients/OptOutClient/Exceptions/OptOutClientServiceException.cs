// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.OptOutClient.Exceptions
{
    public class OptOutClientServiceException : Xeption
    {
        public OptOutClientServiceException(Xeption innerException)
            : base(message: "Opt out client service error occurred, fix errors and try again.",
                  innerException)
        { }
    }
}
