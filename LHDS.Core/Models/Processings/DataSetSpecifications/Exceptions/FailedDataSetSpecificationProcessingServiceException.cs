// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.DataSetSpecifications.Exceptions
{
    public class FailedDataSetSpecificationProcessingServiceException : Xeption
    {
        public FailedDataSetSpecificationProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
