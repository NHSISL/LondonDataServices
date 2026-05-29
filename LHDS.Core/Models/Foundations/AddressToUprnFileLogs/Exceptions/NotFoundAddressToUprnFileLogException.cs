// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions
{
    public class NotFoundAddressToUprnFileLogException : Xeption
    {
        public NotFoundAddressToUprnFileLogException(Guid addressToUprnFileLogId)
            : base(message: $"Couldn't find address to UPRN file log with id: {addressToUprnFileLogId}.")
        { }
    }
}
