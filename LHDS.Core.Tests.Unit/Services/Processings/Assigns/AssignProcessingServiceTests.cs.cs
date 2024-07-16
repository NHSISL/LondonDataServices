// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Services.Foundations.Assigns;
using LHDS.Core.Services.Processings.Assigns;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Processings.Assigns
{
    public partial class AssignProcessingServiceTests
    {
        private readonly Mock<IAssignService> assignServiceMock = new Mock<IAssignService>();
        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IAssignProcessingService assignProcessingService;

        public AssignProcessingServiceTests()
        {
            this.assignServiceMock = new Mock<IAssignService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.assignProcessingService = new AssignProcessingService(
                assignService: this.assignServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static AssignAddress CreateRandomAssignAddress() =>
            CreateAssignAddressFiller().Create();

        private static Filler<AssignAddress> CreateAssignAddressFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            var filler = new Filler<AssignAddress>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }
    }
}
