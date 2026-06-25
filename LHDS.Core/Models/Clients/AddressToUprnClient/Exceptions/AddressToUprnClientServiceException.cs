// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.AddressToUprnClient.Exceptions
{
    public class AddressToUprnClientServiceException : Xeption
    {
        public AddressToUprnClientServiceException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
