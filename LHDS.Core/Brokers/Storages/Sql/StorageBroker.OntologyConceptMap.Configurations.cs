// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.OntologyConceptMaps;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddOntologyConceptMapConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OntologyConceptMap>()
                .ToTable("OntologyConceptMaps", "Ontology");

            modelBuilder.Entity<OntologyConceptMap>()
                .Property(ontologyConceptMap => ontologyConceptMap.Id)
                .IsRequired();

            modelBuilder.Entity<OntologyConceptMap>()
                .Property(ontologyConceptMap => ontologyConceptMap.FullUrl)
                .HasMaxLength(1000)
                .IsRequired();

            modelBuilder.Entity<OntologyConceptMap>()
                .HasIndex(ontologyConceptMap => ontologyConceptMap.FullUrl)
                .IsUnique();

            modelBuilder.Entity<OntologyConceptMap>()
                .Property(ontologyConceptMap => ontologyConceptMap.ResourceType)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyConceptMap>()
                .Property(ontologyConceptMap => ontologyConceptMap.Version)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyConceptMap>()
                .Property(ontologyConceptMap => ontologyConceptMap.Name)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyConceptMap>()
                .Property(ontologyConceptMap => ontologyConceptMap.Title)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyConceptMap>()
                .Property(ontologyConceptMap => ontologyConceptMap.Status)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OntologyConceptMap>()
                .Property(ontologyConceptMap => ontologyConceptMap.IsCore)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<OntologyConceptMap>()
                .Property(ontologyConceptMap => ontologyConceptMap.IsDownloaded)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<OntologyConceptMap>()
                .Property(ontologyConceptMap => ontologyConceptMap.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<OntologyConceptMap>()
                .Property(ontologyConceptMap => ontologyConceptMap.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<OntologyConceptMap>()
                .Property(ontologyConceptMap => ontologyConceptMap.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<OntologyConceptMap>()
                .Property(ontologyConceptMap => ontologyConceptMap.UpdatedDate)
                .IsRequired();
        }
    }
}