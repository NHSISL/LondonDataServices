// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.Core.Models.Foundations.OptOuts;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.OptOuts
{
    [Collection(nameof(ApiTestCollection))]
    public partial class OptOutApiTests
    {
        private readonly ApiBroker apiBroker;

        public OptOutApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private static List<OptOut> CreateRandomOptOuts(int count, DateTimeOffset dateTimeOffset)
        {
            List<OptOut> optOuts = new List<OptOut>();

            for (int i = 0; i < count; i++)
            {
                var reference = Guid.NewGuid();

                var optOut = new OptOut
                {
                    Id = reference,
                    NhsNumber = GenerateValidNhsNumber(),
                    Status = "Unknown",
                    UniqueReference = reference.ToString(),
                    CacheTime = dateTimeOffset.AddDays(-50),
                    LastSentToMesh = dateTimeOffset.AddDays(-50),
                    CreatedDate = dateTimeOffset.AddSeconds(i),
                    CreatedBy = "System",
                    UpdatedDate = dateTimeOffset.AddSeconds(i),
                    UpdatedBy = "System",
                };

                optOuts.Add(optOut);
            }

            return optOuts.OrderBy(optOut => optOut.CreatedDate).ToList();
        }

        private static List<OptOutIdentifier> CreateRandomOptOutIdentifiersList()
        {
            return CreateOptOutIdentifierFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: 1)
                    .ToList();
        }

        private static Filler<OptOutIdentifier> CreateOptOutIdentifierFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<OptOutIdentifier>();

            filler.Setup()
                .OnProperty(optOut => optOut.NhsNumber).Use(GenerateValidNhsNumber())
                .OnProperty(optOut => optOut.UniqueReference).Use(GetRandomString())
                .OnProperty(optOut => optOut.Status).Use(GetRandomString())
                .OnProperty(optOut => optOut.StatusChangedDateTime).Use(GetRandomDateTimeOffset());
            return filler;
        }

        private static List<OptOut> CreateRandomOptOutsList(
            int count,
            DateTimeOffset dateTimeOffset,
            string batchReference
            )
        {
            var optOuts = new List<OptOut>();

            for (int i = 0; i < count; i++)
            {
                var optOut = CreateOptOutFiller(dateTimeOffset).Create();
                optOut.BatchReference = batchReference;
                optOuts.Add(optOut);
            }

            return optOuts;
        }

        private static Filler<OptOut> CreateOptOutFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<OptOut>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(optOut => optOut.NhsNumber).Use(GenerateValidNhsNumber())
                .OnProperty(optOut => optOut.Status).Use("Unknown")
                .OnProperty(optOut => optOut.CreatedBy).Use(user)
                .OnProperty(optOut => optOut.UpdatedBy).Use(user);

            return filler;
        }

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
    }
}
