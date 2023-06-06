// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Pds.Exceptions
{
    public class PdsValidationException : Xeption
    {
        public PdsValidationException(Xeption innerException)
            : base(
                message: "Pds validation errors occurred, please try again.",
                innerException)
        { }
    }
}
