// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class MeshValidationException : Xeption
    {
        public MeshValidationException(string message, Xeption? innerException)
            : base(message,innerException)
        { }
    }
}
