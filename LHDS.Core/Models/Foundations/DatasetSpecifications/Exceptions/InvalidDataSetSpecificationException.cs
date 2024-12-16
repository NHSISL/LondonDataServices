// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions
{
    public class InvalidDataSetSpecificationException : Xeption
    {
        public InvalidDataSetSpecificationException(string message)
            : base(message)
        { }
    }
}