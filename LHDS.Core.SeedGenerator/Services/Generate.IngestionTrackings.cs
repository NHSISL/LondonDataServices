// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.Suppliers;
using Tynamix.ObjectFiller;

namespace LHDS.Core.SeedGenerator.Services
{
    public partial class Generate
    {
        private static List<IngestionTracking> CreateRandomIngestionTrackingRecords(
            int recordCount,
            List<Supplier> suppliers)
        {
            List<IngestionTracking> ingestionTrackings = new List<IngestionTracking>();

            foreach (Supplier supplier in suppliers)
            {
                List<IngestionTracking> generatedItems =
                    CreateIngestionTrackingFiller(dateTimeOffset: DateTimeOffset.UtcNow, supplier)
                        .Create(count: recordCount)
                            .ToList();

                ingestionTrackings.AddRange(generatedItems);
            }

            return ingestionTrackings;
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset dateTimeOffset,
            Supplier supplier)
        {
            Guid id = Guid.NewGuid();
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.Id).Use(id)
                .OnProperty(ingestionTracking => ingestionTracking.FileName).Use(GetRandomString(maxCharacters: 450))
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplier.Id)
                .OnProperty(ingestionTracking => ingestionTracking.EncryptedFileName).Use(GetRandomString(maxCharacters: 450))
                .OnProperty(ingestionTracking => ingestionTracking.DecryptedFileName).Use(GetRandomString(maxCharacters: 450))
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user);

            return filler;
        }
    }
}
