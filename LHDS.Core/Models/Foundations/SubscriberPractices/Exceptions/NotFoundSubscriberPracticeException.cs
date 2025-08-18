// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions
{
    public class NotFoundSubscriberPracticeException : Xeption
    {
        public NotFoundSubscriberPracticeException(Guid subscriberPracticeId)
            : base(message: $"Couldn't find subscriberPractice with subscriberPracticeId: {subscriberPracticeId}.")
        { }
    }
}