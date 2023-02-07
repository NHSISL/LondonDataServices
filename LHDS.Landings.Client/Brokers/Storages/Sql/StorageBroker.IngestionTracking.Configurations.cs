// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Landings.Client.Brokers.Storages.Sql
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