// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddSubscriberAgreementConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubscriberAgreement>()
                .ToTable("SubscriberAgreements", "Configurations");

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.SupplierSharingAgreementShortName)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<SubscriberAgreement>()
                .HasIndex(subscriberAgreement => subscriberAgreement.SupplierSharingAgreementShortName)
                .IsUnique();

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.SupplierSharingAgreementGuid)
                .IsRequired(false);

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.FtpUserName)
                .HasMaxLength(128)
                .IsRequired(false);

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.FtpPublicKey)
                .HasMaxLength(128)
                .IsRequired(false);

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.GpgPublicKey)
                .HasMaxLength(128)
                .IsRequired(false);

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.IsActive)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.LastPollStartDate)
                .IsRequired(false);

            modelBuilder.Entity<SubscriberAgreement>()
                .Property(subscriberAgreement => subscriberAgreement.LastPollEndDate)
                .IsRequired(false);

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