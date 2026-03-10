// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.FileNameValidations.Exceptions
{
    public class InvalidArgumentFileNameValidationServiceException : Xeption
    {
        public InvalidArgumentFileNameValidationServiceException(string message)
            : base(message)
        { }
    }
}
