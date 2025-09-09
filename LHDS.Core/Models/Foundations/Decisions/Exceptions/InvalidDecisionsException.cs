// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Decisions.Exceptions
{
    public class InvalidDecisionsException : Xeption
    {
        public InvalidDecisionsException(string message)
            : base(message)
        { }
    }
}
