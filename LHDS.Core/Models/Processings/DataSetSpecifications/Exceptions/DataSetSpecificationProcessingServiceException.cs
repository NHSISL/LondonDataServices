// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.DataSetSpecifications.Exceptions
{
    public class DataSetSpecificationProcessingServiceException : Xeption
    {
        public DataSetSpecificationProcessingServiceException(string message, Xeption? innerException)
          : base(message, innerException)
        { }
    }
}
