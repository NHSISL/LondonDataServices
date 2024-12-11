// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ObjectColumns.Exceptions
{
    public class NullObjectColumnException : Xeption
    {
        public NullObjectColumnException(string message)
            : base(message)
        { }
    }
}