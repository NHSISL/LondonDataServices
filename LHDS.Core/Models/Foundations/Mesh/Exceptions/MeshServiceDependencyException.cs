// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class MeshServiceDependencyException : Xeption
    {
        public MeshServiceDependencyException(Xeption innerException)
            : base(message: "Mesh service dependency error occurred, contact support.") { }
    }
}
