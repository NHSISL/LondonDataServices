// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using LHDS.Core.Services.Foundations.Mesh;
using LHDS.Core.Services.Processings.Mesh;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        private readonly Mock<IMeshService> meshServiceMock = new Mock<IMeshService>();
        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IMeshProcessingService meshProcessingService;

        public MeshProcessingServiceTests()
        {
            this.meshProcessingService = new MeshProcessingService(
                this.meshServiceMock.Object,
                this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        public static TheoryData<Xeption> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new MeshValidationException(
                    message: "Mesh validation errors occurred, please try again.",
                    innerException),

                new MeshDependencyValidationException(
                    message: "Mesh dependency validation occurred, please try again.",
                    innerException)
            };
        }

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new MeshDependencyException(
                    message: "Mesh dependency error occurred, please contact support.",
                    innerException),

                new MeshServiceException(
                    message: "Mesh service error occurred, please contact support.",
                    innerException)
            };
        }

        private static MeshMessage CreateRandomMessage() =>
            CreateMessageFiller().Create();

        private static MeshMessage CreateRandomSendMessage()
        {
            MeshMessage message = CreateMessageFiller().Create();
            message.MessageId = null;
            message.TrackingInfo = null;

            return message;
        }

        private static Filler<MeshMessage> CreateMessageFiller()
        {
            var filler = new Filler<MeshMessage>();
            filler.Setup().OnProperty(message => message.Headers)
                .Use(new Dictionary<string, List<string>>());

            return filler;
        }
    }
}
