// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using LHDS.Core.Services.Coordinations.EmisLandings;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.EmisLandings
{
    public partial class EmisLandingsControllerTests : RESTFulController
    {
        private readonly Mock<IEmisLandingCoordinationService> emisLandingCoordinationServiceMock;
        private readonly EmisLandingsController emisLandingsController;

        public EmisLandingsControllerTests()
        {
            this.emisLandingCoordinationServiceMock = new Mock<IEmisLandingCoordinationService>();
            this.emisLandingsController = new EmisLandingsController(this.emisLandingCoordinationServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        public static TheoryData<Xeption> ServerExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new EmisLandingOrchestrationDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new EmisLandingOrchestrationServiceException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }

        public static TheoryData<Xeption> ValidationExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new EmisLandingOrchestrationValidationException(
                    message: someMessage,
                    innerException: someInnerException),

                new EmisLandingOrchestrationDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
    }
}
