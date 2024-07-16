// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AssignAddresses.Exceptions
{
    public class InvalidArgumentAssignAddressProcessingException : Xeption
    {
        public InvalidArgumentAssignAddressProcessingException(string message)
            : base(message)
        { }
    }
}