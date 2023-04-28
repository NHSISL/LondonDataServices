// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class MeshServiceDependencyValidationException : Xeption
    {
        public MeshServiceDependencyValidationException(Xeption innerException)
            : base(message: "Mesh service dependency validation occurred, please try again.") { }
    }
}
