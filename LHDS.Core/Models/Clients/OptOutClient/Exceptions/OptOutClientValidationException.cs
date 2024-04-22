// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.OptOutClient.Exceptions
{
    public class OptOutClientValidationException : Xeption
    {
        public OptOutClientValidationException(Xeption innerException)
            : base(
                message: "Opt out client validation error occurred, fix errors and try again.",
                innerException)
        { }
    }
}
