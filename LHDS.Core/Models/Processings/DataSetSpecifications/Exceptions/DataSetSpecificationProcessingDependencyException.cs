// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.DataSetSpecifications.Exceptions
{
    public class DataSetSpecificationProcessingDependencyException : Xeption
    {
        public DataSetSpecificationProcessingDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}
