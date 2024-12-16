// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class SpecificationObjectValidationException : Xeption
    {
        public SpecificationObjectValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}