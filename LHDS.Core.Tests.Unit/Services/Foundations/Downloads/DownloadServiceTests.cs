// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Downloads;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DownloadServiceTests
    {
        private readonly Mock<IDownloadBroker> downloadBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IDownloadService downloadService;

        public DownloadServiceTests()
        {
            this.downloadBrokerMock = new Mock<IDownloadBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.downloadService = new DownloadService(
                downloadBroker: this.downloadBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        public static TheoryData MinutesBeforeOrAfter()
        {
            int randomNumber = GetRandomNumber();
            int randomNegativeNumber = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                randomNumber,
                randomNegativeNumber
            };
        }

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static List<Document> CreateRandomDocuments()
        {
            return CreateDocumentFiller()
                .Create(count: GetRandomNumber())
                    .ToList();
        }

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