// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class NullMessageException : Xeption
    {
        public NullMessageException()
            : base(message: "Message is null.") { }
    }
}
