// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.TppLandings.Exceptions
{
    public class FailedTppLandingCoordinationServiceException : Xeption
    {
        public FailedTppLandingCoordinationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}