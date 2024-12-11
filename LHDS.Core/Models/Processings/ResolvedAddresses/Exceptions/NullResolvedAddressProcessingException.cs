// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions
{
    public class NullResolvedAddressProcessingException : Xeption
    {
        public NullResolvedAddressProcessingException(string message)
            : base(message)
        { }
    }
}
