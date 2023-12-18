// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Tpp.Exceptions
{
    public class InvalidArgumentException : Xeption
    {
        public InvalidArgumentException(string message)
            : base(message)
        { }
    }
}