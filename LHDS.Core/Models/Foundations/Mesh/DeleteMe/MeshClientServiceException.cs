// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.DeleteMe
{
    internal class MeshClientServiceException : Xeption
    {
        public MeshClientServiceException(Xeption innerException)
            : base(message: "Mesh client dependency validation error occurred, contact support.", innerException)
        { }
    }
}
