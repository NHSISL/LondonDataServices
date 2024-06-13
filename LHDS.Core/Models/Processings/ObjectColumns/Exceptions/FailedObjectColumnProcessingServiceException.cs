// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.ObjectColumns.Exceptions
{
    public class FailedObjectColumnProcessingServiceException : Xeption
    {
        public FailedObjectColumnProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
