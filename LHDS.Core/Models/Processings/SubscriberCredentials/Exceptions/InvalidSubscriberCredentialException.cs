// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions
{
    public class InvalidSubscriberCredentialException : Xeption
    {
        public InvalidSubscriberCredentialException(string message)
            : base(message)
        { }
    }
}