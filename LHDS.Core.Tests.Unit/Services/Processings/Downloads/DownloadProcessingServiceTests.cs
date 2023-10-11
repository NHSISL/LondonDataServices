// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Processings.Downloads;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Downloads
{
    public partial class DownloadProcessingServiceTests
    {
        private readonly Mock<IDownloadService> downloadServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IDownloadProcessingService downloadProcessingService;

        public DownloadProcessingServiceTests()
        {
            this.downloadServiceMock = new Mock<IDownloadService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.downloadProcessingService = new DownloadProcessingService(
                downloadService: this.downloadServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        public static TheoryData DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DownloadValidationException(
                    message: "Download validation errors occurred, please try again.", innerException),

                new DownloadDependencyValidationException(
                    message: "Download dependency validation occurred, please try again.", innerException)
            };
        }

        public static TheoryData DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DownloadDependencyException(
                    message: "Download validation errors occurred, please try again.", innerException),

                new DownloadServiceException(
                    message : "Download service error occurred, contact support.", innerException)
            };
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static List<Document> CreateRandomDocuments() =>
            CreateDocumentFiller().Create(count: GetRandomNumber()).ToList();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static Document CreateRandomDocument() =>
            CreateDocumentFiller().Create();

        private static Filler<Document> CreateDocumentFiller()
        {
            var filler = new Filler<Document>();
            filler.Setup();

            return filler;
        }
    }
}