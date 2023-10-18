// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressNormalisation.Exceptions
{
    public class AddressNormalisationDependencyValidationException : Xeption
    {
        public AddressNormalisationDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
