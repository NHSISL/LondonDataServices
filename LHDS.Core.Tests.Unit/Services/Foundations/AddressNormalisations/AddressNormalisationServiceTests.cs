// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using LHDS.Core.Brokers.AddressNormalisations;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AddressNormalisation;
using LHDS.Core.Services.Foundations.AddressNormalisations;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressNormalisations
{
    public partial class AddressNormalisationServiceTests
    {
        private readonly Mock<IAddressNormalisationBroker> addressNormalisationBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IAddressNormalisationService addressNormalisationService;

        public AddressNormalisationServiceTests()
        {
            this.addressNormalisationBrokerMock = new Mock<IAddressNormalisationBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.addressNormalisationService = new AddressNormalisationService(
                addressNormalisationBroker: this.addressNormalisationBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static AddressNormalisation CreateRandomAddressNormalisation() =>
            CreateAddressNormalisationFiller().Create();

        private static Filler<AddressNormalisation> CreateAddressNormalisationFiller()
        {
            var filler = new Filler<AddressNormalisation>();

            return filler;
        }
    }
}