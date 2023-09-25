// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Mesh.Exceptions
{
    public class MeshProcessingDependencyException : Xeption
    {
        public MeshProcessingDependencyException(string message, Xeption innerException) :
            base(message: "Mesh processing dependency error occurred, contact support.", innerException)
        { }
    }
}
