// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Mesh.Exceptions
{
    public class NullMeshMessageProcessingException : Xeption
    {
        public NullMeshMessageProcessingException(string message)
            : base(message)
        { }
    }
}
