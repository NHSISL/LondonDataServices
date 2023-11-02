// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.OntologyPolls;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddOntologyPollConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OntologyPoll>()
                .ToTable("OntologyPolls", "Ontology");

            modelBuilder.Entity<OntologyPoll>()
                .Property(ontologyPoll => ontologyPoll.Id)
                .IsRequired();

            modelBuilder.Entity<OntologyPoll>()
                .Property(ontologyPoll => ontologyPoll.ResourceType)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyPoll>()
                .HasIndex(ontologyPoll => ontologyPoll.ResourceType)
                .IsUnique();

            modelBuilder.Entity<OntologyPoll>()
                .Property(ontologyPoll => ontologyPoll.LastPoll)
                .IsRequired(false);

            modelBuilder.Entity<OntologyPoll>()
                .Property(ontologyPoll => ontologyPoll.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<OntologyPoll>()
                .Property(ontologyPoll => ontologyPoll.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<OntologyPoll>()
                .Property(ontologyPoll => ontologyPoll.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<OntologyPoll>()
                .Property(ontologyPoll => ontologyPoll.UpdatedDate)
                .IsRequired();
        }
    }
}