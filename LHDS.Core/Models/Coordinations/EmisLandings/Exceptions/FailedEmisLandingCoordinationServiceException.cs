// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.EmisLandings.Exceptions
{
    public class FailedEmisLandingCoordinationServiceException : Xeption
    {
        public FailedEmisLandingCoordinationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}