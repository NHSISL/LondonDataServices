// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions
{
    public class NullSubscriberPracticeException : Xeption
    {
        public NullSubscriberPracticeException(string message)
            : base(message)
        { }
    }
}