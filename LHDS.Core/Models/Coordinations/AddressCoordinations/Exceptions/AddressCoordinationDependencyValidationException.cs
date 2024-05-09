// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions
{
    public class AddressCoordinationDependencyValidationException : Xeption
    {
        public AddressCoordinationDependencyValidationException(string message, Xeption? innerException)
         : base(message, innerException)
        { }
    }
}
