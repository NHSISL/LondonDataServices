// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions
{
    public class SubscriberPracticeDependencyException : Xeption
    {
        public SubscriberPracticeDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}