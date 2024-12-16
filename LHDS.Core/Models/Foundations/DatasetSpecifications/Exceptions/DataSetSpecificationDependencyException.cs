// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions
{
    public class DataSetSpecificationDependencyException : Xeption
    {
        public DataSetSpecificationDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}