// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions
{
    public class NullSubscriberCredentialException : Xeption
    {
        public NullSubscriberCredentialException(string message)
            : base(message)
        { }
    }
}