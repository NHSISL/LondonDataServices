// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions
{
    public class LockedIngestionTrackingException : Xeption
    {
        public LockedIngestionTrackingException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}