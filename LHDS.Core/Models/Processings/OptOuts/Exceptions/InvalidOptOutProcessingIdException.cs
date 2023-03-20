// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class InvalidOptOutProcessingIdException : Xeption
    {
        public InvalidOptOutProcessingIdException()
            : base(message: "Invalid opt out processing Id. Please correct the errors and try again.") 
        { }
    }
}
