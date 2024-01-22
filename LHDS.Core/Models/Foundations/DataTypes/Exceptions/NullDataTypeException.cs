// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.DataTypes.Exceptions
{
    public class NullDataTypeException : Xeption
    {
        public NullDataTypeException(string message)
            : base(message)
        { }
    }
}