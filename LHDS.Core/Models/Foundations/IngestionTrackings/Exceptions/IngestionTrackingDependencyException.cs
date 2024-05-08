// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions
{
    public class IngestionTrackingDependencyException : Xeption
    {
        public IngestionTrackingDependencyException(string message, Xeption? innerException)
            : base(message, innerException) 
        { }
    }
}
