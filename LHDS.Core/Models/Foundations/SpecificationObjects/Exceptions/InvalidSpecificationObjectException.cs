// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class InvalidSpecificationObjectException : Xeption
    {
        public InvalidSpecificationObjectException(string message)
            : base(message)
        { }
    }
}