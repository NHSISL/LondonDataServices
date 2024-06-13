// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions
{
    public class AddressCoordinationValidationException : Xeption
    {
        public AddressCoordinationValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
