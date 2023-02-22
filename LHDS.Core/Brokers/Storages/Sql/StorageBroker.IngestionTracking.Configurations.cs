// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.IngestionTrackings;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddIngestionTrackingConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.Id)
                .HasMaxLength(450)
                .IsRequired();

            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.Source)
                .HasMaxLength(450)
                .IsRequired();

            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.EncryptedFileName)
                .HasMaxLength(450)
                .IsRequired();

            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.DecryptedFileName)
                .HasMaxLength(450)
                .IsRequired();
        }
    }
}