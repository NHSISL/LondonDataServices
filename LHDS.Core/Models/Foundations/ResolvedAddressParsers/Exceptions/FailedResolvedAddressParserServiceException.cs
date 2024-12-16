// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddressParsers.Exceptions
{
    public class FailedResolvedAddressParserServiceException : Xeption
    {
        public FailedResolvedAddressParserServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}