using System;
using System.Linq.Expressions;
using Moq;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Services.Foundations.AddressLoadingAudits;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IAddressLoadingAuditService addressLoadingAuditService;

        public AddressLoadingAuditServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.addressLoadingAuditService = new AddressLoadingAuditService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static AddressLoadingAudit CreateRandomAddressLoadingAudit(DateTimeOffset dateTimeOffset) =>
            CreateAddressLoadingAuditFiller(dateTimeOffset).Create();

        private static Filler<AddressLoadingAudit> CreateAddressLoadingAuditFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<AddressLoadingAudit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(addressLoadingAudit => addressLoadingAudit.CreatedBy).Use(user)
                .OnProperty(addressLoadingAudit => addressLoadingAudit.UpdatedBy).Use(user);

            // TODO: Complete the filler setup e.g. ignore related properties etc...

            return filler;
        }
    }
}