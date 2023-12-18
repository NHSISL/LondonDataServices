// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Tpp.Exceptions
{
    public class NullTppDocumentException : Xeption
    {
        public NullTppDocumentException(string message)
            : base(message)
        { }
    }
}
