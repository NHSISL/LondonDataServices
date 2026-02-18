// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.FileNameValidations.Exceptions
{
    public class InvalidArgumentFileNameException : Xeption
    {
        public InvalidArgumentFileNameException(string message)
            : base(message)
        { }
    }
}
