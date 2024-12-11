// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Mesh.Exceptions
{
    public class InvalidMeshMessageProcessingException : Xeption
    {
        public InvalidMeshMessageProcessingException(string message)
            : base(message)
        { }
    }
}
