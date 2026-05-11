// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberPractices;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<SubscriberPractice> InsertSubscriberPracticeAsync(
            SubscriberPractice subscriberPractice,
            CancellationToken cancellationToken = default);

        ValueTask<IQueryable<SubscriberPractice>> SelectAllSubscriberPracticesAsync(
            CancellationToken cancellationToken = default);

        ValueTask<SubscriberPractice> SelectSubscriberPracticeByIdAsync(
            Guid subscriberPracticeId,
            CancellationToken cancellationToken = default);

        ValueTask<SubscriberPractice> UpdateSubscriberPracticeAsync(
            SubscriberPractice subscriberPractice,
            CancellationToken cancellationToken = default);

        ValueTask<SubscriberPractice> DeleteSubscriberPracticeAsync(
            SubscriberPractice subscriberPractice,
            CancellationToken cancellationToken = default);
    }
}