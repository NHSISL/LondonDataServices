// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Coordinations.EmisLandings.Exceptions
{
    public class NullSupplierCredentialEmisLandingCoordinationException : Xeption
    {
        public NullSupplierCredentialEmisLandingCoordinationException(string message)
            : base(message)
        { }
    }
}