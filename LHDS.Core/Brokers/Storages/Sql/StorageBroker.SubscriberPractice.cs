// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<SubscriberPractice> SubscriberPractices { get; set; }

        public async ValueTask<SubscriberPractice> InsertSubscriberPracticeAsync(
            SubscriberPractice subscriberPractice,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(subscriberPractice, cancellationToken);

        public async ValueTask<IQueryable<SubscriberPractice>> SelectAllSubscriberPracticesAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<SubscriberPractice>(cancellationToken);

        public async ValueTask<SubscriberPractice> SelectSubscriberPracticeByIdAsync(
            Guid subscriberPracticeId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<SubscriberPractice>(new object[] { subscriberPracticeId }, cancellationToken);

        public async ValueTask<SubscriberPractice> UpdateSubscriberPracticeAsync(
            SubscriberPractice subscriberPractice,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(subscriberPractice, cancellationToken);

        public async ValueTask<SubscriberPractice> DeleteSubscriberPracticeAsync(
            SubscriberPractice subscriberPractice,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(subscriberPractice, cancellationToken);
    }
}