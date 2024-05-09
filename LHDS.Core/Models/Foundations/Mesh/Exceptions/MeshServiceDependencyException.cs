// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class MeshServiceDependencyException : Xeption
    {
        public MeshServiceDependencyException(string message, Xeption? innerException)
            : base(message, innerException) 
        { }
    }
}
