// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class NullSpecificationObjectException : Xeption
    {
        public NullSpecificationObjectException(string message)
            : base(message)
        { }
    }
}