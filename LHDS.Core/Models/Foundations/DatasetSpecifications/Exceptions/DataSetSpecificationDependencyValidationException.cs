// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions
{
    public class DataSetSpecificationDependencyValidationException : Xeption
    {
        public DataSetSpecificationDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}