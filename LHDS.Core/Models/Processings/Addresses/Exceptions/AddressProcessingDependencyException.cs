// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Addresses.Exceptions
{
    public class AddressProcessingDependencyException : Xeption
    {
        public AddressProcessingDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}
