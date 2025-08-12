// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions
{
    public class InvalidSubscriberPracticeReferenceException : Xeption
    {
        public InvalidSubscriberPracticeReferenceException(string message, Exception? innerException)
            : base(message, innerException) { }
    }
}