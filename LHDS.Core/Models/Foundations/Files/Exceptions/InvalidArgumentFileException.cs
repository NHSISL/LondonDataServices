// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Files.Exceptions
{
    public class InvalidArgumentFileException : Xeption
    {
        public InvalidArgumentFileException(string message)
            : base(message)
        { }
    }
}
