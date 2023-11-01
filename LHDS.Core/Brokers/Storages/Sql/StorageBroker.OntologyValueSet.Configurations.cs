// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.OntologyValueSets;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddOntologyValueSetConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OntologyValueSet>()
                .ToTable("OntologyValueSets", "Ontology");

            modelBuilder.Entity<OntologyValueSet>()
                .Property(ontologyValueSet => ontologyValueSet.Id)
                .IsRequired();

            modelBuilder.Entity<OntologyValueSet>()
                .Property(ontologyValueSet => ontologyValueSet.FullUrl)
                .HasMaxLength(1000)
                .IsRequired();

            modelBuilder.Entity<OntologyValueSet>()
                .HasIndex(ontologyValueSet => ontologyValueSet.FullUrl)
                .IsUnique();

            modelBuilder.Entity<OntologyValueSet>()
                .Property(ontologyValueSet => ontologyValueSet.ResourceType)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyValueSet>()
                .Property(ontologyValueSet => ontologyValueSet.Version)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyValueSet>()
                .Property(ontologyValueSet => ontologyValueSet.Name)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyValueSet>()
                .Property(ontologyValueSet => ontologyValueSet.Title)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyValueSet>()
                .Property(ontologyValueSet => ontologyValueSet.Status)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyValueSet>()
                .Property(ontologyValueSet => ontologyValueSet.IsCore)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<OntologyValueSet>()
                .Property(ontologyValueSet => ontologyValueSet.IsDownloaded)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<OntologyValueSet>()
                .Property(ontologyValueSet => ontologyValueSet.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<OntologyValueSet>()
                .Property(ontologyValueSet => ontologyValueSet.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<OntologyValueSet>()
                .Property(ontologyValueSet => ontologyValueSet.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<OntologyValueSet>()
                .Property(ontologyValueSet => ontologyValueSet.UpdatedDate)
                .IsRequired();
        }
    }
}