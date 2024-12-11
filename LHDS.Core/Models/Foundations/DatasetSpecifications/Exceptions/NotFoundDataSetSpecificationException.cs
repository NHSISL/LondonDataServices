// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions
{
    public class NotFoundDataSetSpecificationException : Xeption
    {
        public NotFoundDataSetSpecificationException(Guid dataSetSpecificationId)
            : base(message: $"Couldn't find dataSetSpecification with dataSetSpecificationId: {dataSetSpecificationId}.")
        { }
    }
}