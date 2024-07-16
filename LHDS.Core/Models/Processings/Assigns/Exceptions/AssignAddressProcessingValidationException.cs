// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AssignAddresses.Exceptions
{
    public class AssignAddressProcessingValidationException : Xeption
    {
        public AssignAddressProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
