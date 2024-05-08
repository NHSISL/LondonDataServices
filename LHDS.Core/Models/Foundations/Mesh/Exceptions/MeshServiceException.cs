// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Mesh.Exceptions
{
    public class MeshServiceException : Xeption
    {
        public MeshServiceException(string message, Exception? innerException)
           : base(message, innerException)
        { }
    }
}
