// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.Decryptions.Exceptions
{
    public class FailedDecryptionCoordinationServiceException : Xeption
    {
        public FailedDecryptionCoordinationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}