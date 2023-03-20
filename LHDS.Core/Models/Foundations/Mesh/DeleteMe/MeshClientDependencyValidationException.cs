// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.DeleteMe
{
    internal class MeshClientDependencyValidationException : Xeption
    {
        public MeshClientDependencyValidationException(Xeption innerException)
            : base(message: "Mesh client dependency validation error occurred, contact support.", innerException)
        { }
    }
}
