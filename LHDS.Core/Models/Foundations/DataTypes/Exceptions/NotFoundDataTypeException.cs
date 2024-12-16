// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class NotFoundDataTypeException : Xeption
    {
        public NotFoundDataTypeException(Guid dataTypeId)
            : base(message: $"Couldn't find dataType with dataTypeId: {dataTypeId}.")
        { }
    }
}