// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class FailedMeshServiceException : Xeption
    {
        public FailedMeshServiceException(Exception innerException)
            : base(message: "Failed mesh service occurred, please contact support", innerException)
        { }
    }
}