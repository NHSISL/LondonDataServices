// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.TppLandings.Exceptions
{
    public class TppLandingCoordinationServiceException : Xeption
    {
        public TppLandingCoordinationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}