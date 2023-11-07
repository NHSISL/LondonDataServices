// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using LHDS.Core.Services.Processings.TerminologyPolls;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyPolls
{
    public partial class TerminologyPollProcessingServiceTests
    {
        private readonly Mock<ITerminologyPollService> terminologyPollServiceMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITerminologyPollProcessingService terminologyPollProcessingService;

        public TerminologyPollProcessingServiceTests()
        {
            this.terminologyPollServiceMock = new Mock<ITerminologyPollService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.terminologyPollProcessingService = new TerminologyPollProcessingService(
                terminologyPollService: this.terminologyPollServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

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

        private static TerminologyPoll CreateRandomTerminologyPoll() =>
            CreateTerminologyPollFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static TerminologyPoll CreateRandomTerminologyPoll(DateTimeOffset dateTimeOffset) =>
            CreateTerminologyPollFiller(dateTimeOffset).Create();

        private static Filler<TerminologyPoll> CreateTerminologyPollFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<TerminologyPoll>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(terminologyPoll => terminologyPoll.CreatedBy).Use(user)
                .OnProperty(terminologyPoll => terminologyPoll.UpdatedBy).Use(user);

            return filler;
        }
    }
}