// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class LockedDataTypeException : Xeption
    {
        public LockedDataTypeException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}