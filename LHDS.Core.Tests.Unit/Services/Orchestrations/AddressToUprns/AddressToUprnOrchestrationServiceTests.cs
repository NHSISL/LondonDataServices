// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.AssignAddresses.AssignABPAddresses;
using LHDS.Core.Models.Foundations.AssignAddresses.AssignMatchPatterns;
using LHDS.Core.Models.Foundations.AssignAddresses.BestMatch;
using LHDS.Core.Models.Foundations.Assigns.Exceptions;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Orchestrations.AddressToUprns;
using LHDS.Core.Services.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Services.Foundations.Assigns;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Orchestrations.AddressToUprns;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressToUprns
{
    public partial class AddressToUprnOrchestrationServiceTests
    {
        private readonly ITestOutputHelper output;
        private readonly Mock<IAssignService> assignServiceMock;
        private readonly Mock<IDocumentService> documentServiceMock;
        private readonly Mock<IAddressToUprnFileLogService> addressToUprnFileLogServiceMock;
        private readonly Mock<ICsvHelperBroker> csvHelperBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly BlobContainers blobContainers;
        private readonly AddressToUprnConfiguration addressToUprnConfiguration;
        private readonly IAddressToUprnOrchestrationService addressToUprnOrchestrationService;

        public AddressToUprnOrchestrationServiceTests(ITestOutputHelper output)
        {
            this.output = output;
            this.assignServiceMock = new Mock<IAssignService>();
            this.documentServiceMock = new Mock<IDocumentService>();
            this.addressToUprnFileLogServiceMock = new Mock<IAddressToUprnFileLogService>();
            this.csvHelperBrokerMock = new Mock<ICsvHelperBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.blobContainers = new BlobContainers
            {
                AddressToUprn = "address-to-uprn"
            };

            this.addressToUprnConfiguration = new AddressToUprnConfiguration
            {
                InboxFolder = "inbox",
                OutboxFolder = "outbox",
                ErrorFolder = "error",
                MaxFileSizeLimitMb = 35840
            };

            this.addressToUprnOrchestrationService = new AddressToUprnOrchestrationService(
                assignService: this.assignServiceMock.Object,
                documentService: this.documentServiceMock.Object,
                addressToUprnFileLogService: this.addressToUprnFileLogServiceMock.Object,
                csvHelperBroker: this.csvHelperBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                auditBroker: null,
                loggingBroker: this.loggingBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object,
                addressToUprnConfiguration: this.addressToUprnConfiguration,
                blobContainers: this.blobContainers);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static Stream CreateCsvStream(string content)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(content);
            MemoryStream stream = new MemoryStream(bytes);

            return stream;
        }

        private static Stream CreateSingleAddressCsvStream(string address = null)
        {
            string unstructuredAddress = address ?? GetRandomString();

            string csv =
                $"UnstructuredAddress\r\n" +
                $"\"{unstructuredAddress}\"\r\n";

            return CreateCsvStream(csv);
        }

        private static AssignAddress CreateRandomAssignAddress(bool matched = true)
        {
            return new AssignAddress
            {
                AddressFormat = GetRandomString(),
                PostcodeQuality = GetRandomString(),
                Matched = matched,
                BestMatch = new BestMatch
                {
                    UPRN = GetRandomString(),
                    Qualifier = GetRandomString(),
                    Classification = GetRandomString(),
                    Algorithm = GetRandomString(),
                    ABPAddress = new AssignABPAddress
                    {
                        Number = GetRandomString(),
                        Street = GetRandomString(),
                        Town = GetRandomString(),
                        Postcode = GetRandomString(),
                        Organisation = GetRandomString()
                    },
                    MatchPattern = new AssignMatchPattern
                    {
                        Flat = GetRandomString(),
                        Building = GetRandomString(),
                        Number = GetRandomString(),
                        Street = GetRandomString(),
                        Postcode = GetRandomString()
                    }
                }
            };
        }

        private static AddressToUprnFileLog CreateRandomAddressToUprnFileLog() =>
            CreateAddressToUprnFileLogFiller().Create();

        private static Filler<AddressToUprnFileLog> CreateAddressToUprnFileLogFiller()
        {
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();
            var filler = new Filler<AddressToUprnFileLog>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use((DateTimeOffset?)dateTimeOffset);

            return filler;
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 5).GetValue();

        private static Guid GetRandomGuid() =>
            Guid.NewGuid();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        public static TheoryData<Xeption> AddressToUprnDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            var innerException = new Xeption(randomMessage);

            return new TheoryData<Xeption>
            {
                new AssignValidationException(
                    message: "Assign validation errors occurred, please try again.",
                    innerException),

                new AssignDependencyValidationException(
                    message: "Assign dependency validation errors occurred, please try again.",
                    innerException),

                new DocumentValidationException(
                    message: "Document validation errors occurred, please try again.",
                    innerException),

                new DocumentDependencyValidationException(
                    message: "Document dependency validation occurred, please try again.",
                    innerException),

                new AddressToUprnFileLogValidationException(
                    message: "Address to UPRN file log validation errors occurred, please try again.",
                    innerException),

                new AddressToUprnFileLogDependencyValidationException(
                    message: "Address to UPRN file log dependency validation errors occurred, please try again.",
                    innerException)
            };
        }

        public static TheoryData<Xeption> AddressToUprnDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            var innerException = new Xeption(randomMessage);

            return new TheoryData<Xeption>
            {
                new AssignDependencyException(
                    message: "Assign dependency error occurred, please try again.",
                    innerException),

                new AssignServiceException(
                    message: "Assign service error occurred, please contact support.",
                    innerException),

                new DocumentDependencyException(
                    message: "Document dependency error occurred, please try again.",
                    innerException),

                new DocumentServiceException(
                    message: "Document service error occurred, please contact support.",
                    innerException),

                new AddressToUprnFileLogDependencyException(
                    message: "Address to UPRN file log dependency error occurred, please try again.",
                    innerException),

                new AddressToUprnFileLogServiceException(
                    message: "Address to UPRN file log service error occurred, please contact support.",
                    innerException)
            };
        }

        private static string GetRandomStringOfLength(int length)
        {
            string result = new MnemonicString(
                wordCount: 1,
                wordMinLength: length,
                wordMaxLength: length).GetValue();

            return result.Length > length ? result[..length] : result;
        }
    }
}
