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
using LHDS.Core.Brokers.Identifiers;
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
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
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
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.compareLogic = new CompareLogic();

            this.optOutOrchestrationService = new OptOutOrchestrationService(
                optOutProcessingService: this.optOutProcessingServiceMock.Object,
                documentProcessingService: this.documentProcessingServiceMock.Object,
                meshProcessingService: this.meshProcessingServiceMock.Object,
                csvMapperProcessingService: this.csvMapperProcessingServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object,
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
            new DateTimeRange(earliestDate: new DateTime().AddDays(7)).GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static List<OptOutIdentifier> CreateRandomOptOutIdentifiersList()
        {
            return CreateOptOutIdentifierFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .ToList();
        }
        private static List<OptOut> CreateRandomOptOutsList()
        {
            return CreateOptOutFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private string CreateNewCsvList(List<OptOutIdentifier> differentIdentifiers, bool hasTrailingComma)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var item in differentIdentifiers)
            {
                if (hasTrailingComma)
                {
                    builder.AppendLine($"{item},");
                }
                else
                {
                    builder.AppendLine($"{item}");
                }
            }

            return builder.ToString();
        }

        private static List<OptOut> CreateRandomOptOutsList(
            List<OptOutIdentifier> items,
            string batchReference,
            string status)
        {
            var identifiers = new List<OptOut>();

            foreach (var item in items)
            {
                var optOut = CreateOptOutFiller(GetRandomDateTimeOffset()).Create();
                optOut.NhsNumber = item.NhsNumber;
                optOut.OptOutStatus = status;
                optOut.BatchReference = batchReference;
                identifiers.Add(optOut);
            }

            return identifiers;
        }

        private static List<OptOutIdentifier> CreateRandomListOfOptOutIdentifiers(int count)
        {
            var identifiers = new List<OptOutIdentifier>();

            for (int i = 0; i < count; i++)
            {
                identifiers.Add(CreateOptOutIdentifierFiller().Create());
            }

            return identifiers;
        }

        private static List<string> GetRandomStrings(int count)
        {
            var messages = new List<string>();

            for (int i = 0; i < count; i++)
            {
                string message = new MnemonicString(wordCount: GetRandomNumber()).GetValue();
                messages.Add(message);
            }

            return messages;
        }

        private static List<MeshMessage> GetRandomMessages(List<string> items, string batchReference)
        {
            List<MeshMessage> messageList = new List<MeshMessage>();

            foreach (var item in items)
            {
                var message = CreateRandomMessage();
                message.MessageId = item;
                message.Headers["Mex-LocalID"] = new List<string> { batchReference };
                messageList.Add(message);
            }

            return messageList;
        }

        private static OptOut CreateRandomOptOut(DateTimeOffset dateTimeOffset) =>
           CreateOptOutFiller(dateTimeOffset).Create();

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

        private Expression<Func<OptOut, bool>> SameOptOutAs(
            OptOut expectedOptOut)
        {
            return actualOptOut =>
                this.compareLogic.Compare(expectedOptOut, actualOptOut)
                    .AreEqual;
        }

        private Expression<Func<MeshMessage, bool>> SameMessageAs(
            MeshMessage expectedMessage)
        {
            return actualMessage =>
                this.compareLogic.Compare(expectedMessage, actualMessage)
                    .AreEqual;
        }

        private Expression<Func<string, bool>> SameStringAs(
            string expectedString)
        {
            return actualString =>
                this.compareLogic.Compare(expectedString, actualString)
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
                .OnProperty(optOut => optOut.OptOutStatus).Use("Unknown")
                .OnProperty(optOut => optOut.CreatedBy).Use(user)
                .OnProperty(optOut => optOut.UpdatedBy).Use(user);

            return filler;
        }

        private static Filler<OptOutIdentifier> CreateOptOutIdentifierFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<OptOutIdentifier>();

            filler.Setup()
                .OnProperty(optOut => optOut.NhsNumber).Use(GenerateValidNhsNumber());

            return filler;
        }

        private static Filler<OptOutIdentifier> CreateOptOutIdentifierFiller()
        {
            var filler = new Filler<OptOutIdentifier>();

            filler.Setup()
                .OnProperty(optOut => optOut.NhsNumber).Use(GenerateValidNhsNumber());

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

        public static Dictionary<string, List<string>> CreateMandatoryHeaders()
        {
            var dictionary = new Dictionary<string, List<string>>
            {
                { "Content-Type", new List<string> { GetRandomString() } },
                { "Mex-FileName", new List<string> { GetRandomString() } },
                { "Mex-From", new List<string> { GetRandomString() } },
                { "Mex-To", new List<string> { GetRandomString() } },
                { "Mex-WorkflowID", new List<string> { GetRandomString() } },
                { "Mex-Content-Checksum", new List<string> { GetRandomString() } },
                { "Mex-Content-Encrypted", new List<string> { GetRandomString() } },
                { "Mex-LocalID", new List<string> { GetRandomString() } }
            };

            return dictionary;
        }

        private static MeshMessage CreateRandomMessage() =>
           CreateMessageFiller().Create();

        private static Filler<MeshMessage> CreateMessageFiller()
        {
            var filler = new Filler<MeshMessage>();
            filler.Setup().OnProperty(message => message.Headers)
                .Use(CreateMandatoryHeaders());

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
