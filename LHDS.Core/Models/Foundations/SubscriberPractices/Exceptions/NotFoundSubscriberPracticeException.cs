// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions
{
    public class NotFoundSubscriberPracticeException : Xeption
    {
        public NotFoundSubscriberPracticeException(string message)
            : base(message)
        { }
    }
}