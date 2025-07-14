// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.SubscriberPractices;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddSubscriberPracticeConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubscriberPractice>()
                .ToTable("SubscriberPractices", "Configuration");

            modelBuilder.Entity<SubscriberPractice>()
                .ToTable(subscriberPractice => subscriberPractice.IsTemporal());

            modelBuilder.Entity<SubscriberPractice>()
               .Property(subscriberPractice => subscriberPractice.Id)
               .IsRequired();

            modelBuilder.Entity<SubscriberPractice>()
                .Property(subscriberPractice => subscriberPractice.SubscriberAgreementId)
                .IsRequired();

            modelBuilder.Entity<SubscriberPractice>()
                .Property(subscriberPractice => subscriberPractice.Name)
                .HasMaxLength(250)
                .IsRequired();

            modelBuilder.Entity<SubscriberPractice>()
                .Property(subscriberPractice => subscriberPractice.PracticeCode)
                .HasMaxLength(250)
                .IsRequired();


            modelBuilder.Entity<SubscriberPractice>()
                .Property(subscriberPractice => subscriberPractice.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<SubscriberPractice>()
                .Property(subscriberPractice => subscriberPractice.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<SubscriberPractice>()
                .Property(subscriberPractice => subscriberPractice.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<SubscriberPractice>()
                .Property(subscriberPractice => subscriberPractice.UpdatedDate)
                .IsRequired();

            modelBuilder.Entity<SubscriberPractice>()
                .HasOne(subscriberPractice => subscriberPractice.SubscriberAgreement);
        }
    }
}