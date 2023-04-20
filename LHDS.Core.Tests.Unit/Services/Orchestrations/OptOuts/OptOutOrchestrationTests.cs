// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Orchestrations.OptOuts;
using LHDS.Core.Models.Processings.CsvMapper.Exceptions;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using LHDS.Core.Services.Orchestrations.OptOuts;
using LHDS.Core.Services.Processings.CsvMappers;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Mesh;
using LHDS.Core.Services.Processings.OptOuts;
using Microsoft.Extensions.Configuration;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        private readonly Mock<IOptOutProcessingService> optOutProcessingServiceMock;
        private readonly Mock<IDocumentProcessingService> documentProcessingServiceMock;
        private readonly Mock<IMeshProcessingService> meshProcessingServiceMock;
        private readonly Mock<ICsvMapperProcessingService> csvMapperProcessingServiceMock;
        private readonly OptOutOrchestrationService optOutOrchestrationService;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly OptOutConfiguration optOutConfiguration;
        private readonly IConfiguration inMemoryConfiguration;

        public OptOutOrchestrationTests()
        {
            var appSettingsStub = new Dictionary<string, string> {
                {"OptOutSettings:ExpiredAfterDays", "7"},
                {"OptOutSettings:InputFolder", GetRandomString()},
                {"OptOutSettings:OptOutFileHasHeader", "false"},
                {"OptOutSettings:OutputFolder", GetRandomString()},
                {"OptOutSettings:OptOutFileRequireTrailingComma", "true"},
            };

            this.inMemoryConfiguration = new ConfigurationBuilder()
               .AddInMemoryCollection(appSettingsStub)
               .Build();

            this.optOutConfiguration = new OptOutConfiguration
            {
                ExpiredAfterDays = int.Parse(inMemoryConfiguration["OptOutSettings:ExpiredAfterDays"]),
                InputFolder = inMemoryConfiguration["OptOutSettings:InputFolder"],
                OptOutFileHasHeader = bool.Parse(inMemoryConfiguration["OptOutSettings:OptOutFileHasHeader"]),
                OutputFolder = inMemoryConfiguration["OptOutSettings:OutputFolder"],

                OptOutFileRequireTrailingComma =
                    bool.Parse(inMemoryConfiguration["OptOutSettings:OptOutFileRequireTrailingComma"]),
            };

            this.optOutProcessingServiceMock = new Mock<IOptOutProcessingService>();
            this.documentProcessingServiceMock = new Mock<IDocumentProcessingService>();
            this.meshProcessingServiceMock = new Mock<IMeshProcessingService>();
            this.csvMapperProcessingServiceMock = new Mock<ICsvMapperProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.compareLogic = new CompareLogic();

            this.optOutOrchestrationService = new OptOutOrchestrationService(
                optOutProcessingService: this.optOutProcessingServiceMock.Object,
                documentProcessingService: this.documentProcessingServiceMock.Object,
                meshProcessingService: this.meshProcessingServiceMock.Object,
                csvMapperProcessingService: this.csvMapperProcessingServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                optOutConfiguration: this.optOutConfiguration);
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

        private static List<OptOut> CreateRandomOptOutsList()
        {
            return CreateOptOutFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: 1)
                    .ToList();
        }
        private static OptOut CreateRandomOptOut() =>
             CreateOptOutFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private Expression<Func<List<OptOut>, bool>> SameOptOutListAs(List<OptOut> expectedOptOuts)
        {
            return actualOptOuts =>
                this.compareLogic.Compare(expectedOptOuts, actualOptOuts)
                    .AreEqual;
        }

        private static IQueryable<OptOut> CreateRandomOptOuts()
        {
            return CreateOptOutFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private Expression<Func<Document, bool>> SameDocumentAs(
            Document expectedDocument)
        {
            return actualDocument =>
                this.compareLogic.Compare(expectedDocument, actualDocument)
                    .AreEqual;
        }

        private Expression<Func<MeshMessage, bool>> SameMessageAs(
            MeshMessage expectedMessage)
        {
            return actualMessage =>
                this.compareLogic.Compare(expectedMessage, actualMessage)
                    .AreEqual;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

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

        public static TheoryData OptOutDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new OptOutProcessingValidationException(innerException),
                new OptOutProcessingDependencyValidationException(innerException),
                new DocumentProcessingValidationException(innerException),
                new DocumentProcessingDependencyValidationException(innerException),
                new MeshProcessingValidationException(innerException),
                new MeshProcessingDependencyValidationException(innerException),
                new CsvMapperProcessingValidationException(innerException),
                new CsvMapperProcessingDependencyValidationException(innerException),
            };
        }

        private static MeshMessage CreateRandomMessage() =>
           CreateMessageFiller().Create();

        private static Filler<MeshMessage> CreateMessageFiller()
        {
            var filler = new Filler<MeshMessage>();
            filler.Setup().OnProperty(message => message.Headers)
                .Use(new Dictionary<string, List<string>>());

            return filler;
        }

        public static TheoryData OptOutDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new OptOutProcessingDependencyException(innerException),
                new OptOutProcessingServiceException(innerException),
                new DocumentProcessingDependencyException(innerException),
                new DocumentProcessingServiceException(innerException),
                new MeshProcessingDependencyException(innerException),
                new MeshProcessingServiceException(innerException),
                new CsvMapperProcessingDependencyException(innerException),
                new CsvMapperProcessingServiceException(innerException)
            };
        }

        private string GetValidationSummary(IDictionary data)
        {
            StringBuilder validationSummary = new StringBuilder();

            foreach (DictionaryEntry entry in data)
            {
                string errorSummary = ((List<string>)entry.Value)
                    .Select((string value) => value)
                    .Aggregate((string current, string next) => current + ", " + next);

                validationSummary.Append($"{entry.Key} => {errorSummary};  ");
            }

            return validationSummary.ToString();
        }
    }
}
