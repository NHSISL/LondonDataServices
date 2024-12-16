// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ObjectColumns.Exceptions
{
    public class AlreadyExistsObjectColumnException : Xeption
    {
        public AlreadyExistsObjectColumnException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}