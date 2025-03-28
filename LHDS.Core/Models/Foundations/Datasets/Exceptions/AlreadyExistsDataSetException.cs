// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSets.Exceptions
{
    public class AlreadyExistsDataSetException : Xeption
    {
        public AlreadyExistsDataSetException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}