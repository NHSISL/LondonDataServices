// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions
{
    public class NullDataSetSpecificationException : Xeption
    {
        public NullDataSetSpecificationException(string message)
            : base(message)
        { }
    }
}