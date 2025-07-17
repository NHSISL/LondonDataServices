// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<SubscriberPractice> SubscriberPractices { get; set; }

        public async ValueTask<SubscriberPractice> InsertSubscriberPracticeAsync(
            SubscriberPractice subscriberPractice) =>
                await InsertAsync(subscriberPractice);

        public ValueTask<IQueryable<SubscriberPractice>> SelectAllSubscriberPracticesAsync() => 
            SelectAllAsync<SubscriberPractice>();

        public async ValueTask<SubscriberPractice> SelectSubscriberPracticeByIdAsync(Guid subscriberPracticeId) =>
            await SelectAsync<SubscriberPractice>(subscriberPracticeId);

        public async ValueTask<SubscriberPractice> UpdateSubscriberPracticeAsync(
            SubscriberPractice subscriberPractice) =>
                await UpdateAsync(subscriberPractice);

        public async ValueTask<SubscriberPractice> DeleteSubscriberPracticeAsync(
            SubscriberPractice subscriberPractice) =>
                await DeleteAsync(subscriberPractice);
    }
}