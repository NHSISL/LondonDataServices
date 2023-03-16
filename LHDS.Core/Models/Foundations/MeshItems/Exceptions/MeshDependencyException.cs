// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.MeshItems.Exceptions
{
    public class MeshDependencyException : Xeption
    {
        public MeshDependencyException(Xeption innerException) :
            base(message: "Mesh dependency error occurred, contact support.", innerException)
        { }
    }
}
