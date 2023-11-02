// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.OntologyCodeSystems;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddOntologyCodeSystemConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OntologyCodeSystem>()
                .ToTable("OntologyCodeSystems", "Ontology");

            modelBuilder.Entity<OntologyCodeSystem>()
                .Property(ontologyCodeSystem => ontologyCodeSystem.Id)
                .IsRequired();

            modelBuilder.Entity<OntologyCodeSystem>()
                .Property(ontologyCodeSystem => ontologyCodeSystem.FullUrl)
                .HasMaxLength(1000)
                .IsRequired();

            modelBuilder.Entity<OntologyCodeSystem>()
                .HasIndex(ontologyCodeSystem => ontologyCodeSystem.FullUrl)
                .IsUnique();

            modelBuilder.Entity<OntologyCodeSystem>()
                .Property(ontologyCodeSystem => ontologyCodeSystem.ResourceType)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyCodeSystem>()
                .Property(ontologyCodeSystem => ontologyCodeSystem.Version)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyCodeSystem>()
                .Property(ontologyCodeSystem => ontologyCodeSystem.Name)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyCodeSystem>()
                .Property(ontologyCodeSystem => ontologyCodeSystem.Title)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyCodeSystem>()
                .Property(ontologyCodeSystem => ontologyCodeSystem.Status)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyCodeSystem>()
                .Property(ontologyCodeSystem => ontologyCodeSystem.IsCore)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<OntologyCodeSystem>()
                .Property(ontologyCodeSystem => ontologyCodeSystem.IsDownloaded)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<OntologyCodeSystem>()
                .Property(ontologyCodeSystem => ontologyCodeSystem.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<OntologyCodeSystem>()
                .Property(ontologyCodeSystem => ontologyCodeSystem.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<OntologyCodeSystem>()
                .Property(ontologyCodeSystem => ontologyCodeSystem.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<OntologyCodeSystem>()
                .Property(ontologyCodeSystem => ontologyCodeSystem.UpdatedDate)
                .IsRequired();
        }
    }
}