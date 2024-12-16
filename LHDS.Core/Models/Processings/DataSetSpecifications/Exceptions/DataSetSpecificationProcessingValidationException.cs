// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.DataSetSpecifications.Exceptions
{
    public class DataSetSpecificationProcessingValidationException : Xeption
    {
        public DataSetSpecificationProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
