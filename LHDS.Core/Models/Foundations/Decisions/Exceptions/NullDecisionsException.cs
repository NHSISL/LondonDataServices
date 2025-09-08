// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Decisions.Exceptions
{
    public class NullDecisionsException : Xeption
    {
        public NullDecisionsException(string message)
            : base(message)
        { }
    }
}