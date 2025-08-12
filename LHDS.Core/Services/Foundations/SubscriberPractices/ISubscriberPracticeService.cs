// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberPractices;

namespace LHDS.Core.Services.Foundations.SubscriberPractices
{
    public interface ISubscriberPracticeService
    {
        ValueTask<SubscriberPractice> AddSubscriberPracticeAsync(SubscriberPractice subscriberPractice);
        ValueTask<IQueryable<SubscriberPractice>> RetrieveAllSubscriberPracticesAsync();
        ValueTask<SubscriberPractice> RetrieveSubscriberPracticeByIdAsync(Guid subscriberPracticeId);
        ValueTask<SubscriberPractice> ModifySubscriberPracticeAsync(SubscriberPractice subscriberPractice);
        ValueTask<SubscriberPractice> RemoveSubscriberPracticeByIdAsync(Guid subscriberPracticeId);
    }
}