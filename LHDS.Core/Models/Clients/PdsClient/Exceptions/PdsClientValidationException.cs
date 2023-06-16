// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.Pds.Exceptions
{
    public class PdsClientValidationException : Xeption
    {
        public PdsClientValidationException(Xeption innerException)
            : base(
                message: "PDS client validation error occurred, fix errors and try again.",
                innerException)
        { }
    }
}
