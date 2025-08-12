// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions
{
    public class InvalidSubscriberPracticeException : Xeption
    {
        public InvalidSubscriberPracticeException(string message)
            : base(message)
        { }
    }
}