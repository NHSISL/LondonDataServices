// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Mesh.Exceptions
{
    public class InvalidMeshProcessingArgumentException : Xeption
    {
        public InvalidMeshProcessingArgumentException()
               : base(message: "Invalid mesh processing argument. Please correct the errors and try again.")
        { }
    }
}
