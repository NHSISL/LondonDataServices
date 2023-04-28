// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class MeshValidationException : Xeption
    {
        public MeshValidationException(Xeption innerException)
            : base(
                message: "Mesh validation errors occurred, please try again.",
                innerException)
        { }
    }
}
