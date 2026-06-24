// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions
{
    public class AddressToUprnFileLogValidationException : Xeption
    {
        public AddressToUprnFileLogValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
