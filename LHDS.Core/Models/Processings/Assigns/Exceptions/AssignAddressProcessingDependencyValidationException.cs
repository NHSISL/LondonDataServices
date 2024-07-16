// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AssignAddresses.Exceptions
{
    public class AssignAddressProcessingDependencyValidationException : Xeption
    {
        public AssignAddressProcessingDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
