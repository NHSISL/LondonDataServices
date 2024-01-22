// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Addresses.Exceptions
{
    public class NullAddressProcessingException : Xeption
    {
        public NullAddressProcessingException(string message)
            : base(message)
        { }
    }
}
