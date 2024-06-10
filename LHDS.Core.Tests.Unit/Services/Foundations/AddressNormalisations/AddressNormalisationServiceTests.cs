// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Linq.Expressions;
using LHDS.Core.Brokers.AddressNormalisations;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Services.Foundations.AddressNormalisations;
using Moq;
using NHSISL.LibPostalClient.Models.Clients.LibPostal.Exceptions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressNormalisations
{
    public partial class AddressNormalisationServiceTests
    {
        private readonly Mock<IAddressNormalisationBroker> addressNormalisationBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IAddressNormalisationService addressNormalisationService;
        private readonly ITestOutputHelper output;

        public AddressNormalisationServiceTests(ITestOutputHelper output)
        {
            this.output = output;
            this.addressNormalisationBrokerMock = new Mock<IAddressNormalisationBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.addressNormalisationService = new AddressNormalisationService(
                addressNormalisationBroker: this.addressNormalisationBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        public static TheoryData<string> AddressExpansionData()
        {
            return new TheoryData<string>
            {
                "10 Downing Street, Westminster, London, SW1A2AA, United Kingdom",
                "10 Downing Street, , , ,Westminster, London, SW1A2AA, United Kingdom",
                "10 Downing Street,Westminster,London,SW1A2AA,United Kingdom",
                "10 Downing Street,,,,Westminster,London,SW1A2AA,United Kingdom",
                "10 Downing Street, Westminster,  London,   SW1A2AA,     United Kingdom",
                "10 Downing Street, Westminster\n, London\n, SW1A2AA,United Kingdom",
                "10 Downing Street, Westminster\r\n, London\r\n, SW1A2AA,United Kingdom",
                "10 Downing Street\n,Westminster\n,London,SW1A2AA\n,United Kingdom",
                "10 Downing Street\r\n,Westminster\r\n,London,SW1A2AA\r\n,United Kingdom"
            };
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static AddressNormalisation CreateRandomAddressNormalisation() =>
            CreateAddressNormalisationFiller().Create();

        private static Filler<AddressNormalisation> CreateAddressNormalisationFiller()
        {
            var filler = new Filler<AddressNormalisation>();

            return filler;
        }

        public static TheoryData<Xeption> AddressNormalisationDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new LibPostalClientDependencyException(
                    message: "Lib postal client dependency validation error occured, please try again",
                    innerException),

                new LibPostalClientServiceException(
                    message: "Lib postal client service error occured, please try again",
                    innerException)
            };
        }

        public static TheoryData<Xeption> AddressNormalisationDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);
            IDictionary data = null;

            return new TheoryData<Xeption>
            {
                new LibPostalClientValidationException(
                    message: "Lib postal client validation error occurred, please contact support.",
                    innerException,
                    data)
            };
        }
    }
}