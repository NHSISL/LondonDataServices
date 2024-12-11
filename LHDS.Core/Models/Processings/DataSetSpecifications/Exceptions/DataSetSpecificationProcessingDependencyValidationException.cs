// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.DataSetSpecifications.Exceptions
{
    public class DataSetSpecificationProcessingDependencyValidationException : Xeption
    {
        public DataSetSpecificationProcessingDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
