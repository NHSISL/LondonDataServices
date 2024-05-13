// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AddressLoadingAudits.Exceptions
{
    public class AddressLoadingAuditProcessingServiceException : Xeption
    {
        public AddressLoadingAuditProcessingServiceException(string message, Xeption? innerException)
          : base(message, innerException)
        { }
    }
}
