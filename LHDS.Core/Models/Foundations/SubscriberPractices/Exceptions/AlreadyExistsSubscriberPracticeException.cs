// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions
{
    public class AlreadyExistsSubscriberPracticeException : Xeption
    {
        public AlreadyExistsSubscriberPracticeException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}