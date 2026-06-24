// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions
{
    public class FailedAddressToUprnFileLogStorageException : Xeption
    {
        public FailedAddressToUprnFileLogStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
