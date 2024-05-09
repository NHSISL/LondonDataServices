// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.AddressParsers.Exceptions
{
    public class FailedAddressParserProcessingServiceException : Xeption
    {
        public FailedAddressParserProcessingServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}