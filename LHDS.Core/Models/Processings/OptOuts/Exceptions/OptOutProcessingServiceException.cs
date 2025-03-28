// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class OptOutProcessingServiceException : Xeption
    {
        public OptOutProcessingServiceException(string message, Xeption innerException)
          : base(message, innerException)
        { }
    }
}
