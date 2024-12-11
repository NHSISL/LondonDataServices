// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSets.Exceptions
{
    public class FailedDataSetStorageException : Xeption
    {
        public FailedDataSetStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}