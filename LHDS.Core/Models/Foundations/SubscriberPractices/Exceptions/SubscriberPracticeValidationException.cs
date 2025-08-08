// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions
{
    public class SubscriberPracticeValidationException : Xeption
    {
        public SubscriberPracticeValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}