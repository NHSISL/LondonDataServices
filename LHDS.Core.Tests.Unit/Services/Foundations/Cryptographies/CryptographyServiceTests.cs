// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Text;
using LHDS.Core.Brokers.Decryptions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.Cryptographies;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Cryptographies
{
    public partial class CryptographyServiceTests
    {
        private readonly Mock<ICryptographyBroker> cryptographyBroker;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICryptographyService cryptographyService;

        public CryptographyServiceTests()
        {
            this.cryptographyBroker = new Mock<ICryptographyBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.cryptographyService = new CryptographyService(
                cryptographyBroker: this.cryptographyBroker.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomMessage() =>
                new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        public byte[] CreateRandomData()
        {
            string randomMessage = GetRandomMessage();
            return Encoding.ASCII.GetBytes(randomMessage);
        }
    }
}

