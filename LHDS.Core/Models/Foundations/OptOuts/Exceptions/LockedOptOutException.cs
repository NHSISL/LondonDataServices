// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.OptOuts.Exceptions
{
    public class LockedOptOutException : Xeption
    {
        public LockedOptOutException(Exception innerException)
            : base(message: "Locked optOut record exception, please try again later", innerException)
        {
        }
    }
}