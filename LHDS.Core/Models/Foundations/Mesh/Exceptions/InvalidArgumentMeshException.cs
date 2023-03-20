// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class InvalidArgumentMeshException : Xeption
    {
        public InvalidArgumentMeshException()
            : base(message: "Invalid Mesh argument(s), please correct the errors and try again.")
        { }
    }
}
