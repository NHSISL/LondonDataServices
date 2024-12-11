// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class FailedSpecificationObjectServiceException : Xeption
    {
        public FailedSpecificationObjectServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}