// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressParsers.Exceptions
{
    public class AddressParserServiceException : Xeption
    {
        public AddressParserServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}