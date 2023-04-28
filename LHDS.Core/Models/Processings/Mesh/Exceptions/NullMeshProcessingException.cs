// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Mesh.Exceptions
{
    public class NullMeshProcessingException : Xeption
    {
        public NullMeshProcessingException()
            : base(message: $"Mesh processing service exception. Message is Null.")
        { }
    }
}
