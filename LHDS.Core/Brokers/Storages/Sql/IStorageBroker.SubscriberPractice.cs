// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberPractices;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<SubscriberPractice> InsertSubscriberPracticeAsync(SubscriberPractice subscriberPractice);
        ValueTask<IQueryable<SubscriberPractice>> SelectAllSubscriberPracticesAsync();
        ValueTask<SubscriberPractice> SelectSubscriberPracticeByIdAsync(Guid subscriberPracticeId);
        ValueTask<SubscriberPractice> UpdateSubscriberPracticeAsync(SubscriberPractice subscriberPractice);
        ValueTask<SubscriberPractice> DeleteSubscriberPracticeAsync(SubscriberPractice subscriberPractice);
    }
}