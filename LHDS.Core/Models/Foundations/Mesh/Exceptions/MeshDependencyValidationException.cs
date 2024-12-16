// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class MeshDependencyValidationException : Xeption
    {
        public MeshDependencyValidationException(string message, Xeption? innerException)
           : base(message, innerException)
        { }
    }
}
