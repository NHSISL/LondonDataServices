// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.AddressClient.Exceptions
{
    public class AddressClientValidationException : Xeption
    {
        public AddressClientValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
