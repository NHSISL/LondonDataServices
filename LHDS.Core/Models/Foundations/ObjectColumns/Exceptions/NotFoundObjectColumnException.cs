// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ObjectColumns.Exceptions
{
    public class NotFoundObjectColumnException : Xeption
    {
        public NotFoundObjectColumnException(Guid objectColumnId)
            : base(message: $"Couldn't find objectColumn with objectColumnId: {objectColumnId}.")
        { }
    }
}