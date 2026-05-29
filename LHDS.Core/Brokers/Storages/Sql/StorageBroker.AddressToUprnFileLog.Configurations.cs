// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddAddressToUprnFileLogConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressToUprnFileLog>()
                .ToTable("AddressToUprnFileLogs", "Addresses");

            modelBuilder.Entity<AddressToUprnFileLog>()
                .Property(addressToUprnFileLog => addressToUprnFileLog.FileName)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<AddressToUprnFileLog>()
                .HasIndex(addressToUprnFileLog => addressToUprnFileLog.FileName);

            modelBuilder.Entity<AddressToUprnFileLog>()
                .Property(addressToUprnFileLog => addressToUprnFileLog.SuccessStatus)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<AddressToUprnFileLog>()
                .HasIndex(addressToUprnFileLog => addressToUprnFileLog.SuccessStatus);

            modelBuilder.Entity<AddressToUprnFileLog>()
                .Property(addressToUprnFileLog => addressToUprnFileLog.Message)
                .HasMaxLength(2000)
                .IsRequired(false);

            modelBuilder.Entity<AddressToUprnFileLog>()
                .Property(addressToUprnFileLog => addressToUprnFileLog.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<AddressToUprnFileLog>()
                .Property(addressToUprnFileLog => addressToUprnFileLog.CreatedWhen)
                .IsRequired();

            modelBuilder.Entity<AddressToUprnFileLog>()
                .Property(addressToUprnFileLog => addressToUprnFileLog.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<AddressToUprnFileLog>()
                .Property(addressToUprnFileLog => addressToUprnFileLog.UpdatedWhen)
                .IsRequired();
        }
    }
}
