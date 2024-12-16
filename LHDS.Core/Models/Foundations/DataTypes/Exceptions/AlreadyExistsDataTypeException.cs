// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class AlreadyExistsDataTypeException : Xeption
    {
        public AlreadyExistsDataTypeException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}