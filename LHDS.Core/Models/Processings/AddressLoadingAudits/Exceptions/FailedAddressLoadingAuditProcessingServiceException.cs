// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.AddressLoadingAudits.Exceptions
{
    public class FailedAddressLoadingAuditProcessingServiceException : Xeption
    {
        public FailedAddressLoadingAuditProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
