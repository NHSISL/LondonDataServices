// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class NotFoundOptOutException : Xeption
    {
        public NotFoundOptOutException(string message)
            : base(message)
        { }
    }
}