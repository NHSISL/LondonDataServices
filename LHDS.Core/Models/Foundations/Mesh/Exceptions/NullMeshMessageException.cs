// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class NullMeshMessageException : Xeption
    {
        public NullMeshMessageException()
            : base(message: "Mesh message is null.") { }
    }
}
