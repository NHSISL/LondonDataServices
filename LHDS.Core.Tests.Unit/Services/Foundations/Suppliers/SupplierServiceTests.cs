using System;
using System.Linq.Expressions;
using Moq;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages;
using LHDS.Core.Models.Suppliers;
using LHDS.Core.Services.Foundations.Suppliers;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Suppliers
{
    public partial class SupplierServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ISupplierService supplierService;

        public SupplierServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.supplierService = new SupplierService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Supplier CreateRandomSupplier(DateTimeOffset dateTimeOffset) =>
            CreateSupplierFiller(dateTimeOffset).Create();

        private static Filler<Supplier> CreateSupplierFiller(DateTimeOffset dateTimeOffset)
        {
            Guid userId = Guid.NewGuid();
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(supplier => supplier.CreatedByUserId).Use(userId)
                .OnProperty(supplier => supplier.UpdatedByUserId).Use(userId);

            // TODO: Complete the filler setup e.g. ignore related properties etc...

            return filler;
        }
    }
}