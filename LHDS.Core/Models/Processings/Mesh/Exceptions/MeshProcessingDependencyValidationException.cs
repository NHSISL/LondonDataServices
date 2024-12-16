// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Mesh.Exceptions
{
    public class MeshProcessingDependencyValidationException : Xeption
    {
        public MeshProcessingDependencyValidationException(string message, Xeption? innerException)
           : base(message, innerException)
        { }
    }
}
