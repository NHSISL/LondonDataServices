// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions
{
    public class SubscriberPracticeDependencyValidationException : Xeption
    {
        public SubscriberPracticeDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}