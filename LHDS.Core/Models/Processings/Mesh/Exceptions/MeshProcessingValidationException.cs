// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Mesh.Exceptions
{
    public class MeshProcessingValidationException : Xeption
    {
        public MeshProcessingValidationException(Xeption innerException)
            : base(
                message: "Mesh processing validation errors occured, please try again",
                innerException)
        { }
    }
}
