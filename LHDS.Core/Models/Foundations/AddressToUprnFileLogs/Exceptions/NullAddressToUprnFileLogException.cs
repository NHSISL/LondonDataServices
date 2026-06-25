// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions
{
    public class NullAddressToUprnFileLogException : Xeption
    {
        public NullAddressToUprnFileLogException(string message)
            : base(message)
        { }
    }
}
