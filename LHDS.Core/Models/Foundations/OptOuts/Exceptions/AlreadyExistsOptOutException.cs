// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class AlreadyExistsOptOutException : Xeption
    {
        public AlreadyExistsOptOutException(Exception innerException)
            : base(message: "OptOut with the same Id already exists.", innerException)
        { }
    }
}