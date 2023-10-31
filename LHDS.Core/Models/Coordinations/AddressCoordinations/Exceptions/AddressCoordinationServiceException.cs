// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Orchestrations.Decryptions.Exceptions
{
    public class AddressCoordinationServiceException : Xeption
    {
        public AddressCoordinationServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}