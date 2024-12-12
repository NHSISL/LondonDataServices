// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions
{
    public class DataSetSpecificationServiceException : Xeption
    {
        public DataSetSpecificationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}