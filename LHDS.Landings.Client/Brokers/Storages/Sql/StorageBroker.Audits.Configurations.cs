// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Landings.Client.Models.Audits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Landings.Client.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddAuditConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Audit>()
                .Property(audit => audit.IngestionTrackingId)
                .HasMaxLength(450)
                .IsRequired();
        }
    }
}
