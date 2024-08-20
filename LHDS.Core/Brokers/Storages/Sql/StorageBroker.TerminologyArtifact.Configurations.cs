// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddTerminologyArtifactConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TerminologyArtifact>()
                .ToTable("TerminologyArtifacts", "Ontology");

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.Id)
                .IsRequired();

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.FullUrl)
                .HasMaxLength(1000)
                .IsRequired();

            modelBuilder.Entity<TerminologyArtifact>()
                .HasIndex(terminologyArtifact => terminologyArtifact.FullUrl)
                .IsUnique();

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.ResourceType)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.Version)
                .HasMaxLength(100)
                .IsRequired(false);

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.Name)
                .HasMaxLength(500)
                .IsRequired(false);

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.Title)
                .HasMaxLength(500)
                .IsRequired(false);

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.Status)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.IsCore)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.IsDownloaded)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.IsForUser)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.IsDownloadedForUser)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.IsError)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<TerminologyArtifact>()
                .Property(terminologyArtifact => terminologyArtifact.UpdatedDate)
                .IsRequired();
        }
    }
}