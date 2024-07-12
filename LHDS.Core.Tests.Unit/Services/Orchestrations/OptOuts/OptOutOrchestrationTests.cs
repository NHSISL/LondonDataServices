// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Mesh;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Orchestrations.OptOuts;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using LHDS.Core.Services.Orchestrations.OptOuts;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Mesh;
using LHDS.Core.Services.Processings.OptOuts;
using Microsoft.Extensions.Configuration;
using Moq;
using NHSISL.CsvHelperClient.Models.Clients.CsvHelpers.Exceptions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        private readonly Mock<IOptOutProcessingService> optOutProcessingServiceMock;
        private readonly Mock<IDocumentProcessingService> documentProcessingServiceMock;
        private readonly Mock<IMeshProcessingService> meshProcessingServiceMock;
        private readonly OptOutOrchestrationService optOutOrchestrationService;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<ICsvHelperBroker> csvHelperBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly OptOutConfiguration optOutConfiguration;
        private readonly BlobContainers blobContainers;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly MeshConfiguration meshConfiguration;
        private readonly ITestOutputHelper output;

        public OptOutOrchestrationTests(ITestOutputHelper output)
        {
            this.output = output;

            var appSettingsStub = new Dictionary<string, string> {
                { "optOutSettings:expiredAfterDays", "7" },
                { "optOutSettings:inputFolder", GetRandomString() },
                { "optOutSettings:optOutFileHasHeader", "false" },
                { "optOutSettings:outputFolder", GetRandomString() },
                { "optOutSettings:optOutFileRequireTrailingComma", "true" },
                { "optOutSettings:to", GetRandomString() },
                { "optOutSettings:workflowId", GetRandomString() },
                { "meshConfiguration:mailboxId", GetRandomString() },
                { "meshConfiguration:password", GetRandomString() },
                { "meshConfiguration:key", GetRandomString() },
                { "meshConfiguration:url", GetRandomString() },
                { "meshConfiguration:mexClientVersion", GetRandomString() },
                { "meshConfiguration:mexOSName", GetRandomString() },
                { "meshConfiguration:mexOSVersion", GetRandomString() },
                { "meshConfiguration:rootCertificate", null },
                { "meshConfiguration:intermediateCertificates", null },
                { "meshConfiguration:clientCertificate", null },
                { "meshConfiguration:workflowId", GetRandomString() }
            };

            this.inMemoryConfiguration = new ConfigurationBuilder()
               .AddInMemoryCollection(appSettingsStub)
               .Build();

            this.optOutConfiguration = new OptOutConfiguration
            {
                ExpiredAfterDays = int.Parse(inMemoryConfiguration["optOutSettings:expiredAfterDays"]),
                InputFolder = inMemoryConfiguration["optOutSettings:inputFolder"],
                OptOutFileHasHeader = bool.Parse(inMemoryConfiguration["optOutSettings:optOutFileHasHeader"]),
                OutputFolder = inMemoryConfiguration["optOutSettings:outputFolder"],

                OptOutFileRequireTrailingComma =
                    bool.Parse(inMemoryConfiguration["optOutSettings:optOutFileRequireTrailingComma"]),

                To = inMemoryConfiguration["optOutSettings:inputFolder"],
                WorkflowId = inMemoryConfiguration["optOutSettings:workflowId"],
            };

            this.blobContainers = new BlobContainers
            {
                OptOut = "optout"
            };

            this.meshConfiguration = new MeshConfiguration
            {
                MailboxId = inMemoryConfiguration["meshConfiguration:mailboxId"],
                Password = inMemoryConfiguration["meshConfiguration:password"],
                Key = inMemoryConfiguration["meshConfiguration:key"],
                Url = inMemoryConfiguration["meshConfiguration:url"],
                MexClientVersion = inMemoryConfiguration["meshConfiguration:mexClientVersion"],
                MexOSName = inMemoryConfiguration["meshConfiguration:mexOSName"],
                MexOSVersion = inMemoryConfiguration["meshConfiguration:mexOSVersion"],
                RootCertificate = GetCertificate(inMemoryConfiguration["meshConfiguration:rootCertificate"]),

                IntermediateCertificates =
                     GetCertificates(inMemoryConfiguration
                        .GetSection("meshConfiguration:intermediateCertificates")
                            .Get<List<string>>()),

                ClientCertificate = GetCertificate(inMemoryConfiguration["meshConfiguration:clientCertificate"])
            };

            this.optOutProcessingServiceMock = new Mock<IOptOutProcessingService>();
            this.documentProcessingServiceMock = new Mock<IDocumentProcessingService>();
            this.meshProcessingServiceMock = new Mock<IMeshProcessingService>();
            this.csvHelperBrokerMock = new Mock<ICsvHelperBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.compareLogic = new CompareLogic();

            this.optOutOrchestrationService = new OptOutOrchestrationService(
                optOutProcessingService: this.optOutProcessingServiceMock.Object,
                documentProcessingService: this.documentProcessingServiceMock.Object,
                meshProcessingService: this.meshProcessingServiceMock.Object,
                blobContainers: this.blobContainers,
                loggingBroker: this.loggingBrokerMock.Object,
                csvHelperBroker: this.csvHelperBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object,
                optOutConfiguration: this.optOutConfiguration,
                meshConfiguration: this.meshConfiguration);
        }

        private Expression<Func<Stream, bool>> SameStreamAs(Stream expectedStream)
        {
            return actualStream =>
                IsSameStream(expectedStream, actualStream);
        }

        private static bool IsSameStream(Stream expectedStream, Stream actualStream)
        {
            byte[] expectedBytes = ReadAllBytesFromStream(expectedStream);
            byte[] actualBytes = ReadAllBytesFromStream(actualStream);

            return new CompareLogic().Compare(expectedBytes, actualBytes).AreEqual;
        }

        private static byte[] ReadAllBytesFromStream(Stream stream)
        {
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private X509Certificate2Collection GetCertificates(List<string> values)
        {
            var certificates = new X509Certificate2Collection();

            if (values == null || values.Count == 0)
            {
                return certificates;
            }

            foreach (string item in values)
            {
                certificates.Add(GetCertificate(item));
            }

            return certificates;
        }

        private X509Certificate2 GetCertificate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new X509Certificate2();
            }

            byte[] certBytes = Convert.FromBase64String(value);

            return new X509Certificate2(certBytes);
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

        private static string GetHeaderValue(MeshMessage message, string keyToFind)
        {
            List<string> value = new List<string>();

            foreach (var key in message.Headers.Keys)
            {
                if (key.ToLower() == keyToFind.ToLower())
                {
                    message.Headers.TryGetValue(key, out value);

                    break;
                }
            }

            return value.FirstOrDefault();
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
                    builder.AppendLine($"{item.NhsNumber},");
                }
                else
                {
                    builder.AppendLine($"{item.NhsNumber}");
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
                optOut.Status = status;
                optOut.BatchReference = batchReference;
                identifiers.Add(optOut);
            }

            return identifiers;
        }

        private static List<string> CreateRandomListOfConsentedIdentifiers(int count)
        {
            var identifiers = new List<string>();

            for (int i = 0; i < count; i++)
            {
                var item = CreateOptOutIdentifierFiller().Create();
                identifiers.Add(GetRandomString());
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

        private static List<MeshMessage> GetRandomMessages(
            List<string> items,
            List<string> randomConsentedIdentifiers,
            string workflowId = "")
        {
            if (string.IsNullOrWhiteSpace(workflowId))
            {
                workflowId = GetRandomString();
            }

            List<MeshMessage> messageList = new List<MeshMessage>();

            foreach (var item in items)
            {
                StringBuilder sb = new StringBuilder();
                randomConsentedIdentifiers.ForEach(item => sb.AppendLine($"{item},"));

                var message = CreateRandomMessage();
                message.MessageId = item;
                message.Headers["mex-localid"] = new List<string> { GetRandomString() };
                message.FileContent = Encoding.UTF8.GetBytes(sb.ToString());
                message.Headers["mex-workflowid"] = new List<string> { workflowId };

                messageList.Add(message);
            }

            return messageList;
        }

        private Expression<Func<List<string>, bool>> SameStringListAs(List<string> expectedStrings)
        {
            return actualStrings =>
                this.compareLogic.Compare(expectedStrings, actualStrings)
                    .AreEqual;
        }

        private Expression<Func<List<OptOut>, bool>> SameOptOutListAs(List<OptOut> expectedOptOuts)
        {
            return actualOptOuts =>
                this.compareLogic.Compare(expectedOptOuts, actualOptOuts)
                    .AreEqual;
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

        private Expression<Func<List<OptOutIdentifier>, bool>> SameOptOutIdentifierListAs(
           List<OptOutIdentifier> expectedOptOutIdentifierList)
        {

            return actualOptOutIdentifierList =>
                CompareList(expectedOptOutIdentifierList, actualOptOutIdentifierList);
        }

        private bool CompareList(List<OptOutIdentifier> expected, List<OptOutIdentifier> actual)
        {
            var result = compareLogic.Compare(expected, actual).AreEqual;

            return result;
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

        //private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException)
        //{
        //    return actualException =>
        //       CompareExceptions(expectedException, actualException);
        //}

        //private bool CompareExceptions(
        //   Xeption expectedException,
        //   Xeption actualException)
        //{
        //    try
        //    {
        //        actualException.Should().BeEquivalentTo(expectedException);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    return this.compareLogic.Compare(expectedException, actualException).AreEqual;
        //}

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static OptOut CreateRandomOptOut(DateTimeOffset dateTimeOffset) =>
           CreateOptOutFiller(dateTimeOffset).Create();

        private static List<OptOut> CreateRandomOptOuts(int count)
        {
            return CreateOptOutFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count)
                    .ToList();
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

        private static Filler<OptOutIdentifier> CreateOptOutIdentifierFiller()
        {
            var filler = new Filler<OptOutIdentifier>();

            filler.Setup()
                .OnProperty(optOut => optOut.NhsNumber).Use(GenerateValidNhsNumber())
                .OnProperty(optOut => optOut.UniqueReference).Use(GetRandomString())
                .OnProperty(optOut => optOut.Status).Use(GetRandomString())
                .OnProperty(optOut => optOut.StatusChangedDateTime).Use(GetRandomDateTimeOffset());

            return filler;
        }

        public static TheoryData<Xeption> OptOutDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new OptOutProcessingValidationException(innerException),
                new OptOutProcessingDependencyValidationException(innerException),

                new DocumentProcessingValidationException(
                    message: "Document processing validation errors occured, please try again",
                    innerException),

                new DocumentProcessingDependencyValidationException(
                    message: "Document processing dependency validation occurred, please try again.",
                    innerException),

                new MeshProcessingValidationException(
                    message: "Mesh processing validation errors occured, please try again",
                    innerException),

                new MeshProcessingDependencyValidationException(
                    message: "Mesh processing dependency validation occurred, please try again.",
                    innerException),

                new CsvHelperClientValidationException(innerException),
            };
        }

        public static Dictionary<string, List<string>> CreateMandatoryHeaders()
        {
            var dictionary = new Dictionary<string, List<string>>
            {
                { "content-type", new List<string> { GetRandomString() } },
                { "mex-filename", new List<string> { GetRandomString() } },
                { "Mex-From", new List<string> { GetRandomString() } },
                { "mex-to", new List<string> { GetRandomString() } },
                { "mex-workflowid", new List<string> { GetRandomString() } },
                { "mex-content-checksum", new List<string> { GetRandomString() } },
                { "Mex-Content-Encrypted", new List<string> { GetRandomString() } },
                { "mex-localid", new List<string> { GetRandomString() } }
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

        public static TheoryData<Xeption> OptOutDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new OptOutProcessingDependencyException(innerException),
                new OptOutProcessingServiceException(innerException),

                new DocumentProcessingDependencyException(
                    message: "Document processing dependency error occurred, please try again.",
                    innerException),

                new DocumentProcessingServiceException(
                    message: "Document processing service error occurred, please contact support.",
                    innerException),

                new MeshProcessingDependencyException(
                        message: "Mesh processing dependency error occurred, please contact support.",
                        innerException),

                new MeshProcessingServiceException(
                    message: "Mesh processing service error occurred, please contact support.",
                    innerException),

                new CsvHelperClientDependencyException(innerException),
                new CsvHelperClientServiceException(innerException)
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
