// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.DeleteMe
{
    internal class MeshClientDependencyException : Xeption
    {
        public MeshClientDependencyException(Xeption innerException)
            : base(message: "Mesh client dependency error occurred, contact support.", innerException)
        { }
    }
}
