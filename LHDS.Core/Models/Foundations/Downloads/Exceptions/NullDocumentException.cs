// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Downloads.Exceptions
{
    public class NullDocumentException : Xeption
    {
        public NullDocumentException(string message)
            : base(message)
        { }
    }
}