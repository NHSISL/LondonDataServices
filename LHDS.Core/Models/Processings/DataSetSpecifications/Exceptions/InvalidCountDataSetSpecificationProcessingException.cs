// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.DataSetSpecifications.Exceptions
{
    public class InvalidCountDataSetSpecificationProcessingException : Xeption
    {
        public InvalidCountDataSetSpecificationProcessingException(string message)
            : base(message)
        { }
    }
}