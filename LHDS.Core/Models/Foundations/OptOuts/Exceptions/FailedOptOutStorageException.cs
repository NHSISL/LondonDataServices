// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class FailedOptOutStorageException : Xeption
    {
        public FailedOptOutStorageException(Exception innerException)
            : base(message: "Failed optOut storage error occurred, contact support.", innerException)
        { }
    }
}