// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class InvalidArgumentOptOutProcessingException : Xeption
    {
        public InvalidArgumentOptOutProcessingException()
            : base(message: "Invalid opt out processing argument. Please correct the errors and try again.")
        { }
    }
}
