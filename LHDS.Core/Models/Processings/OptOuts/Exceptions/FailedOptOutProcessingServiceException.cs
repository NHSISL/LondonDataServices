// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class FailedOptOutProcessingServiceException : Xeption
    {
        public FailedOptOutProcessingServiceException(Exception innerException)
          : base(message: "Failed opt out processing service error occurred, contact support.",
                innerException)
        { }
    }
}
