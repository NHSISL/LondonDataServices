// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Mesh.Exceptions
{
    public class InvalidMeshProcessingArgumentException : Xeption
    {
        public InvalidMeshProcessingArgumentException(string message)
               : base(message)
        { }
    }
}
