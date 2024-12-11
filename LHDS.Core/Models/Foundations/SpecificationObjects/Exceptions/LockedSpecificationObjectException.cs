// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class LockedSpecificationObjectException : Xeption
    {
        public LockedSpecificationObjectException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}