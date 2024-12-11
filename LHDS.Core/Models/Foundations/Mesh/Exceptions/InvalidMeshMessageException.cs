// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class InvalidMeshMessageException : Xeption
    {
        public InvalidMeshMessageException(string message)
            : base(message)
        { }
    }
}
