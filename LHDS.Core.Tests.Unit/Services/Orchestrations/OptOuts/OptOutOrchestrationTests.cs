// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Services.Orchestrations.OptOuts;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Mesh;
using LHDS.Core.Services.Processings.OptOuts;
using LHDS.Core.Tests.Unit.Services.Processings.CsvMappers;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        private readonly Mock<IOptOutProcessingService> optOutProcessingServiceMock;
        private readonly Mock<IDocumentProcessingService> documentProcessingServiceMock;
        private readonly Mock<IMeshProcessingService> meshProcessingServiceMock;
        private readonly Mock<ICsvMapperProcessingService> csvMapperProcessingServiceMock;
        private readonly OptOutOrchestrationService optOutOrchestrationService;

        public OptOutOrchestrationTests()
        {
            this.optOutProcessingServiceMock = new Mock<IOptOutProcessingService>();
            this.documentProcessingServiceMock = new Mock<IDocumentProcessingService>();
            this.meshProcessingServiceMock = new Mock<IMeshProcessingService>();
            this.csvMapperProcessingServiceMock = new Mock<ICsvMapperProcessingService>();

            this.optOutOrchestrationService = new OptOutOrchestrationService(
                optOutProcessingService: optOutProcessingServiceMock.Object,
                documentProcessingService: documentProcessingServiceMock.Object,
                meshProcessingService: meshProcessingServiceMock.Object,
                csvMapperProcessingService: csvMapperProcessingServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

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

        private static DateTimeOffset GetRandomDateTimeOffset() =>
           new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static List<OptOut> CreateRandomOptOuts()
        {
            return CreateOptOutFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static OptOut CreateRandomOptOut(DateTimeOffset dateTimeOffset) =>
           CreateOptOutFiller(dateTimeOffset).Create();

        private static Filler<OptOut> CreateOptOutFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<OptOut>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(optOut => optOut.NhsNumber).Use(GenerateValidNhsNumber())
                .OnProperty(optOut => optOut.OptOutStatus).Use(GetRandomString())
                .OnProperty(optOut => optOut.CreatedBy).Use(user)
                .OnProperty(optOut => optOut.UpdatedBy).Use(user);

            return filler;
        }
    }
}
