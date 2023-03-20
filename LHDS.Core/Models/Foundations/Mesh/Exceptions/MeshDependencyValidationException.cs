// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.MeshItems.Exceptions
{
    public class MeshDependencyValidationException : Xeption
    {
        public MeshDependencyValidationException(Xeption innerException)
           : base(message: "Mesh dependency validation occurred, please try again.", innerException) { }
    }
}
