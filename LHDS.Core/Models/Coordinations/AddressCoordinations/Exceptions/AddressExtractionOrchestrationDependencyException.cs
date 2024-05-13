// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions
{
    public class AddressCoordinationDependencyException : Xeption
    {
        public AddressCoordinationDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
