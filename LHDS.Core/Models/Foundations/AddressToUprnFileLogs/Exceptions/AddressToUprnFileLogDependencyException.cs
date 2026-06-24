// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions
{
    public class AddressToUprnFileLogDependencyException : Xeption
    {
        public AddressToUprnFileLogDependencyException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
