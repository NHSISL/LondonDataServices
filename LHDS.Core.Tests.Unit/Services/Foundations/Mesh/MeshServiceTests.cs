// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Mesh;
using LHDS.Core.Services.Foundations.Mesh;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        private readonly Mock<IMeshBroker> meshBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IMeshService meshService;

        public MeshServiceTests()
        {
            this.meshBrokerMock = new Mock<IMeshBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.meshService = new MeshService(
                meshBroker: this.meshBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }
        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
           actualException => actualException.SameExceptionAs(expectedException);

        private static int GetRandomNumber() =>
          new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomMessage() =>
          new MnemonicString(wordCount: GetRandomNumber()).GetValue();

    }
}
