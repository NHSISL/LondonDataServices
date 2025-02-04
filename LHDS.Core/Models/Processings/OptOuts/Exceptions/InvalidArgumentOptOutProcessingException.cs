// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class InvalidArgumentOptOutProcessingException : Xeption
    {
        public InvalidArgumentOptOutProcessingException(string message)
            : base(message)
        { }
    }
}
