// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions
{
    public class NullAddressCoordinationListException : Xeption
    {
        public NullAddressCoordinationListException(string message)
            : base(message)
        { }
    }
}