// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
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

        private static IQueryable<TerminologyPoll> CreateRandomTerminologyPolls()
        {
            return CreateTerminologyPollFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
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

        public static TheoryData DependencyValidationExceptions()
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

        public static TheoryData DependencyExceptions()
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