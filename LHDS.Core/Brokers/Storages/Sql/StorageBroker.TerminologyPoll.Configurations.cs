// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.TerminologyPolls;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddTerminologyPollConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TerminologyPoll>()
                .ToTable("TerminologyPolls", "Terminology");

            modelBuilder.Entity<TerminologyPoll>()
                .Property(terminologyPoll => terminologyPoll.Id)
                .IsRequired();

            modelBuilder.Entity<TerminologyPoll>()
                .Property(terminologyPoll => terminologyPoll.ResourceType)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<TerminologyPoll>()
                .HasIndex(terminologyPoll => terminologyPoll.ResourceType)
                .IsUnique();

            modelBuilder.Entity<TerminologyPoll>()
                .Property(terminologyPoll => terminologyPoll.LastPoll)
                .IsRequired(false);

            modelBuilder.Entity<TerminologyPoll>()
                .Property(terminologyPoll => terminologyPoll.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<TerminologyPoll>()
                .Property(terminologyPoll => terminologyPoll.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<TerminologyPoll>()
                .Property(terminologyPoll => terminologyPoll.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<TerminologyPoll>()
                .Property(terminologyPoll => terminologyPoll.UpdatedDate)
                .IsRequired();
        }
    }
}