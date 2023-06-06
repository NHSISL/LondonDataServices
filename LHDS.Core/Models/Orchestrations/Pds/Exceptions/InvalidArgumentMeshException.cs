// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Pds.Exceptions
{
    public class InvalidArgumentPdsException : Xeption
    {
        public InvalidArgumentPdsException()
            : base(message: "Invalid PDS argument(s), please correct the errors and try again.")
        { }
    }
}
