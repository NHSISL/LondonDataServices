// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq.Expressions;
using System;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.AddressNormalisations;
using LHDS.Core.Services.Processings.AddressNormalisations;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
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

        public static TheoryData DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentValidationException(
                    message: "Document validation errors occured, please try again",
                    innerException),

                new DocumentDependencyValidationException(
                    message: "Document dependency validation occurred, please try again.",
                    innerException)
            };
        }
    }
}
