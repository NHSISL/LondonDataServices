// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using LHDS.Core.Services.Foundations.AddressNormalisations;
using LHDS.Core.Services.Processings.AddressNormalisations;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressNormalisations
{
    public partial class AddressNormalisationProcessingServiceTests
    {
        private readonly Mock<IAddressNormalisationService> addressNormalisationServiceMock =
            new Mock<IAddressNormalisationService>();

        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IAddressNormalisationProcessingService addressNormalisationProcessingService;

        public AddressNormalisationProcessingServiceTests()
        {
            this.addressNormalisationProcessingService = new AddressNormalisationProcessingService(
                this.addressNormalisationServiceMock.Object,
                this.loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        public static TheoryData<Xeption> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressNormalisationValidationException(
                    message: "Address Normalisation validation errors occured, please try again",
                    innerException),

                new AddressNormalisationDependencyValidationException(
                    message: "Address Normalisation dependency validation occurred, please try again.",
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
                new AddressNormalisationDependencyException(
                    message: "Address normalisation dependency error occurred, please contact support.",
                    innerException),

                new AddressNormalisationServiceException(
                    message: "Address normalisation service error occurred, please contact support.",
                    innerException)
            };
        }
    }
}
