// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.SpecificationObjects.Exceptions
{
    public class SpecificationObjectProcessingServiceException : Xeption
    {
        public SpecificationObjectProcessingServiceException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
