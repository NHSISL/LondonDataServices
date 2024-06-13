// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class MeshDependencyException : Xeption
    {
        public MeshDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}
