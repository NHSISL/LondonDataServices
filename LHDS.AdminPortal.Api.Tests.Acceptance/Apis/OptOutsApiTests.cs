// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Models.Foundations.OptOuts;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.OptOuts
{
    [Collection(nameof(ApiTestCollection))]
    public partial class OptOutsApiTests
    {
        private readonly ApiBroker apiBroker;

        public OptOutsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: 1, wordMinLength: 2, wordMaxLength: GetRandomNumber()).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GenerateValidNhsNumber()
        {
            int total = 10;
            string formattedNhsNumber = string.Empty;

            while (total == 10)
            {
                var randomNumber = new LongRange(100000000, 999999999);
                formattedNhsNumber = randomNumber.GetValue().ToString();
                int[] multiplers = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int currentNumber;
                int currentSum = 0;
                int currentMultipler;
                string currentString;
                int remainder;

                for (int i = 0; i <= 8; i++)
                {
                    currentString = formattedNhsNumber.Substring(i, 1);

                    currentNumber = Convert.ToInt16(currentString);
                    currentMultipler = multiplers[i];
                    currentSum = currentSum + (currentNumber * currentMultipler);
                }

                remainder = currentSum % 11;
                total = 11 - remainder;

                if (total.Equals(11))
                {
                    total = 0;
                }

                if (total != 10)
                {
                    break;
                }
            }

            string checkNumber = total.ToString();

            return $"{formattedNhsNumber}{checkNumber}";
        }

        private static OptOut UpdateOptOutWithRandomValues(OptOut inputOptOut)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            now.AddSeconds(1);
            var filler = new Filler<OptOut>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(optOut => optOut.Id).Use(inputOptOut.Id)
                .OnProperty(optOut => optOut.BatchReference).Use(inputOptOut.BatchReference)
                .OnProperty(optOut => optOut.UniqueReference).Use(inputOptOut.UniqueReference)
                .OnProperty(optOut => optOut.NhsNumber).Use(inputOptOut.NhsNumber)
                .OnProperty(optOut => optOut.CreatedDate).Use(inputOptOut.CreatedDate)
                .OnProperty(optOut => optOut.CreatedBy).Use(inputOptOut.CreatedBy)
                .OnProperty(optOut => optOut.CacheTime).Use(now.AddDays(-7))
                .OnProperty(optOut => optOut.UpdatedBy).Use(inputOptOut.UpdatedBy)
                .OnProperty(optOut => optOut.UpdatedDate).Use(now);

            return filler.Create();
        }

        private static IQueryable<OptOut> CreateRandomOptOuts()
        {
            List<OptOut> randomOptOuts = new List<OptOut>();
            int randomNumber = GetRandomNumber();

            for (int i = 0; i < randomNumber; i++)
            {
                OptOut optOut = CreateRandomOptOut();

                if (!randomOptOuts.Any(o => o.NhsNumber == optOut.NhsNumber))
                {
                    randomOptOuts.Add(optOut);
                }
            }

            return randomOptOuts.AsQueryable();
        }

        private static OptOut CreateRandomOptOut() =>
            CreateOptOutFiller(dateTimeOffset: GetRandomDateTime()).Create();

        private static Filler<OptOut> CreateOptOutFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<OptOut>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(optOut => optOut.NhsNumber).Use(GenerateValidNhsNumber())
                .OnProperty(optOut => optOut.Status).Use(GetRandomString())
                .OnProperty(optOut => optOut.CreatedBy).Use(user)
                .OnProperty(optOut => optOut.UpdatedBy).Use(user);

            return filler;
        }

        private async ValueTask<OptOut> PostRandomOptOutAsync()
        {
            OptOut randomOptOut = CreateRandomOptOut();
            OptOut storageOptOut = await this.apiBroker.PostOptOutAsync(randomOptOut);

            return storageOptOut;
        }

        private async ValueTask<IngestionTracking> PostRandomIngestionTrackingAsync(Guid supplierId)
        {
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(supplierId);
            await this.apiBroker.PostIngestionTrackingAsync(randomIngestionTracking);

            return randomIngestionTracking;
        }

        private static IngestionTracking CreateRandomIngestionTracking(Guid supplierId) =>
            CreateRandomIngestionTrackingFiller(supplierId).Create();

        private static Filler<IngestionTracking> CreateRandomIngestionTrackingFiller(Guid supplierId)
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplierId)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedDate).Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedDate).Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user);

            return filler;
        }

        private async ValueTask<Supplier> PostRandomSupplierAsync()
        {
            Supplier randomSupplier = CreateRandomSupplier();
            await this.apiBroker.PostSupplierAsync(randomSupplier);

            return randomSupplier;
        }

        private static Supplier CreateRandomSupplier() =>
            CreateRandomSupplierFiller().Create();

        private static Filler<Supplier> CreateRandomSupplierFiller()
        {
            string userId = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(supplier => supplier.CreatedDate).Use(now)
                .OnProperty(supplier => supplier.CreatedBy).Use(userId)
                .OnProperty(supplier => supplier.UpdatedDate).Use(now)
                .OnProperty(supplier => supplier.UpdatedBy).Use(userId);

            return filler;
        }
    }
}