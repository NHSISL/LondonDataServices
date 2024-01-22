// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class InvalidOptOutReferenceException : Xeption
    {
        public InvalidOptOutReferenceException(Exception innerException)
            : base(message: "Invalid optOut reference error occurred.", innerException) { }
    }
}