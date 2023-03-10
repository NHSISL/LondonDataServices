using System;
using System.Linq.Expressions;
using Moq;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages;
using LHDS.Core.Models.OptOuts;
using LHDS.Core.Services.Foundations.OptOuts;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IOptOutService optOutService;

        public OptOutServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.optOutService = new OptOutService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static OptOut CreateRandomOptOut(DateTimeOffset dateTimeOffset) =>
            CreateOptOutFiller(dateTimeOffset).Create();

        private static Filler<OptOut> CreateOptOutFiller(DateTimeOffset dateTimeOffset)
        {
            Guid userId = Guid.NewGuid();
            var filler = new Filler<OptOut>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(optOut => optOut.CreatedByUserId).Use(userId)
                .OnProperty(optOut => optOut.UpdatedByUserId).Use(userId);

            // TODO: Complete the filler setup e.g. ignore related properties etc...

            return filler;
        }
    }
}