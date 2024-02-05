// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddSubscriberAgreementConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubscriberAgreement>()
                .ToTable("SharingAgreements", "Configurations");

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.SupplierSharingAgreementId)
                .IsRequired();

            modelBuilder.Entity<SubscriberAgreement>()
                .HasIndex(subscriberAgreement => subscriberAgreement.SupplierSharingAgreementId)
                .IsUnique();

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.SupplierSharingAgreementGuid)
                .IsRequired();

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.FtpUserName)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.FtpPublicKey)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.GpgPublicKey)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(dataSet => dataSet.IsActive)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(dataSet => dataSet.LastPollStartDate)
                .IsRequired();

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(dataSet => dataSet.LastPollEndDate)
                .IsRequired();

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.UpdatedDate)
                .IsRequired();
        }
    }
}