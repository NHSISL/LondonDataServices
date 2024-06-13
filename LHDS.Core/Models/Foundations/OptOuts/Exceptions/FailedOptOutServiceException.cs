// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class FailedOptOutServiceException : Xeption
    {
        public FailedOptOutServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}