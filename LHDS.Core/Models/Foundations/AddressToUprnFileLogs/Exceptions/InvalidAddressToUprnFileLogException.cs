// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions
{
    public class InvalidAddressToUprnFileLogException : Xeption
    {
        public InvalidAddressToUprnFileLogException(string message)
            : base(message)
        { }
    }
}
