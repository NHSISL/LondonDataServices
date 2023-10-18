// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Services.Foundations.AddressLoadingAudits;
using LHDS.Core.Services.Processings.AddressLoadingAudits;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressLoadingAudits
{
    public partial class AddressLoadingAuditProcessingServiceTests
    {
        private readonly Mock<IAddressLoadingAuditService> addressLoadingAuditServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IAddressLoadingAuditProcessingService addressLoadingAuditProcessingService;

        public AddressLoadingAuditProcessingServiceTests()
        {
            this.addressLoadingAuditServiceMock = new Mock<IAddressLoadingAuditService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.addressLoadingAuditProcessingService = new AddressLoadingAuditProcessingService(
                addressLoadingAuditService: this.addressLoadingAuditServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

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

            return filler;
        }

        public static TheoryData DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressLoadingAuditValidationException(
                    message: "Address loading audit validation errors occurred, please try again.", innerException),

                new AddressLoadingAuditDependencyValidationException(
                    message: "Address loading audit dependency validation occurred, please try again.", innerException)
            };
        }
    }
}