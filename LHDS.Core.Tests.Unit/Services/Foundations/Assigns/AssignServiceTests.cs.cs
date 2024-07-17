// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Assigns;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Services.Foundations.Assigns;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Assigns
{
    public partial class AssignServiceTests
    {
        private readonly Mock<IAssignBroker> assignBrokerMock = new Mock<IAssignBroker>();
        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IAssignService assignService;

        public AssignServiceTests()
        {
            this.assignBrokerMock = new Mock<IAssignBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.assignService = new AssignService(
                assignBroker: this.assignBrokerMock.Object,
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
