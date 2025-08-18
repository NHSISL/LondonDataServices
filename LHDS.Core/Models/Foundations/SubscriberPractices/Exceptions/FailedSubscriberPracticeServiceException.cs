// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions
{
    public class FailedSubscriberPracticeServiceException : Xeption
    {
        public FailedSubscriberPracticeServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}