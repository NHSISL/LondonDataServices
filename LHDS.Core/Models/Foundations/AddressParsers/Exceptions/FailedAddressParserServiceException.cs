// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressParsers.Exceptions
{
    public class FailedAddressParserServiceException : Xeption
    {
        public FailedAddressParserServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}