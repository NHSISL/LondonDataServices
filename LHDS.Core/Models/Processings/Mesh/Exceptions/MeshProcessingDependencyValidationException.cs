// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Mesh.Exceptions
{
    public class MeshProcessingDependencyValidationException : Xeption
    {
        public MeshProcessingDependencyValidationException(Xeption innerException)
           : base(message: "Mesh processing dependency validation occurred, please try again.", innerException)
        { }
    }
}
