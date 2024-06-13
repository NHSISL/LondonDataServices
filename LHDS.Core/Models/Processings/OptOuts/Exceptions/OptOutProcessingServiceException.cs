// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.OptOuts.Exceptions
{
    public class OptOutProcessingServiceException : Xeption
    {
        public OptOutProcessingServiceException(Xeption innerException)
          : base(message: "Opt out processing service error occurred, please contact support.",
                innerException)
        { }
    }
}
