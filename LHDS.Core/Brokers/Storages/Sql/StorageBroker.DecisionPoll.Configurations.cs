// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.DecisionPolls;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddDecisionPollConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DecisionPoll>()
                .ToTable("DecisionPolls", "Decision");

            modelBuilder.Entity<DecisionPoll>()
                .Property(decisionPoll => decisionPoll.Id)
                .IsRequired();

            modelBuilder.Entity<DecisionPoll>()
                .Property(decisionPoll => decisionPoll.LastPoll)
                .IsRequired(true);

            modelBuilder.Entity<DecisionPoll>()
                .Property(decisionPoll => decisionPoll.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DecisionPoll>()
                .Property(decisionPoll => decisionPoll.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<DecisionPoll>()
                .Property(decisionPoll => decisionPoll.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DecisionPoll>()
                .Property(decisionPoll => decisionPoll.UpdatedDate)
                .IsRequired();
        }
    }
}
