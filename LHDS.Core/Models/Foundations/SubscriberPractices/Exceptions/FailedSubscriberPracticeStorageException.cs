// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions
{
    public class FailedSubscriberPracticeStorageException : Xeption
    {
        public FailedSubscriberPracticeStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}