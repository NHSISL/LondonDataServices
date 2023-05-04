// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.OptOuts;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddOptOutConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OptOut>()
                .Property(optOut => optOut.NhsNumber)
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<OptOut>()
                .Property(optOut => optOut.OptOutStatus)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OptOut>()
                .Property(optOut => optOut.BatchReference)
                .HasMaxLength(50)
                .IsRequired(false);
        }
    }
}