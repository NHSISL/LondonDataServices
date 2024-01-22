// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Pds.Exceptions
{
    public class InvalidArgumentPdsException : Xeption
    {
        public InvalidArgumentPdsException(string message)
            : base(message)
        { }
    }
}
