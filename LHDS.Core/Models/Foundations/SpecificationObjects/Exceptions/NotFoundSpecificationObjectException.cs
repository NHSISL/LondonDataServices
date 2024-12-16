// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class NotFoundSpecificationObjectException : Xeption
    {
        public NotFoundSpecificationObjectException(Guid specificationObjectId)
            : base(message: $"Couldn't find specificationObject with specificationObjectId: {specificationObjectId}.")
        { }
    }
}