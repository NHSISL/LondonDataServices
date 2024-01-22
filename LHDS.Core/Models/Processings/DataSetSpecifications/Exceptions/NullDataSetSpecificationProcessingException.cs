// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.DataSetSpecifications.Exceptions
{
    public class NullDataSetSpecificationProcessingException : Xeption
    {
        public NullDataSetSpecificationProcessingException(string message)
            : base(message)
        { }
    }
}
