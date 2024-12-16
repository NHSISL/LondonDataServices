// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class MeshServiceDependencyValidationException : Xeption
    {
        public MeshServiceDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
