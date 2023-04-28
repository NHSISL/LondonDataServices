// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class NullHeadersException : Xeption
    {
        public NullHeadersException()
            : base(message: "Mesh message headers dictionary is null.") { }
    }
}
