// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class FailedOptOutProcessingServiceException : Xeption
    {
        public FailedOptOutProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
