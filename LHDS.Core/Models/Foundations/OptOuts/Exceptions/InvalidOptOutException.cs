// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class InvalidOptOutException : Xeption
    {
        public InvalidOptOutException(string message)
            : base(message)
        { }
    }
}