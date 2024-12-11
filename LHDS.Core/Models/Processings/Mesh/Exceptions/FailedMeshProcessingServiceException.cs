// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.Mesh.Exceptions
{
    public class FailedMeshProcessingServiceException : Xeption
    {
        public FailedMeshProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}

