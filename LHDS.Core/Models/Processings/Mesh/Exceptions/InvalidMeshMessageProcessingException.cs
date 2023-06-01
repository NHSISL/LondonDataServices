// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class InvalidMeshMessageProcessingException : Xeption
    {
        public InvalidMeshMessageProcessingException()
            : base(message: "Invalid mesh message, please correct errors and try again.") { }
    }
}
