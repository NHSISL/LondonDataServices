// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.DataSetSpecifications.Exceptions
{
    public class InvalidArgumentDataSetSpecificationProcessingException : Xeption
    {
        public InvalidArgumentDataSetSpecificationProcessingException(string message)
            : base(message)
        { }
    }
}