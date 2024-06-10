// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using LHDS.Core.Services.Foundations.TerminologyPolls;
using LHDS.Core.Services.Processings.TerminologyPolls;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyPolls
{
    public partial class TerminologyPollProcessingServiceTests
    {
        private readonly Mock<ITerminologyPollService> terminologyPollServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ITerminologyPollProcessingService terminologyPollProcessingService;
        private readonly ICompareLogic compareLogic;

        public TerminologyPollProcessingServiceTests()
        {
            this.terminologyPollServiceMock = new Mock<ITerminologyPollService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            compareLogic = new CompareLogic();

            this.terminologyPollProcessingService = new TerminologyPollProcessingService(
                terminologyPollService: this.terminologyPollServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private Expression<Func<TerminologyPoll, bool>> SameTerminologyPollAs(
            TerminologyPoll expectedTerminologyPoll)
        {
            return actualTerminologyPoll =>
                this.compareLogic.Compare(expectedTerminologyPoll, actualTerminologyPoll)
                    .AreEqual;
        }

        private static TerminologyPoll CreateRandomModifyTerminologyPoll()
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();

            randomTerminologyPoll.CreatedDate =
                randomTerminologyPoll.CreatedDate.AddDays(randomDaysInPast);

            return randomTerminologyPoll;
        }

        private static IQueryable<TerminologyPoll> CreateRandomTerminologyPolls()
        {
            return CreateTerminologyPollFiller()
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static TerminologyPoll CreateRandomTerminologyPoll() =>
            CreateTerminologyPollFiller().Create();

        private static Filler<TerminologyPoll> CreateTerminologyPollFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            var filler = new Filler<TerminologyPoll>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }

        public static TheoryData<Xeption> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new TerminologyPollValidationException(
                    message: "Terminology poll validation error occurred, please try again.", innerException),

                new TerminologyPollDependencyValidationException(
                    message: "Terminology poll dependency validation error occurred, please try again.", innerException)
            };
        }

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new TerminologyPollDependencyException(
                    message: "Terminology poll dependency error occurred, please try again.", innerException),

                new TerminologyPollServiceException(
                    message: "Terminology poll service error occurred, please try again.", innerException)
            };
        }
    }
}