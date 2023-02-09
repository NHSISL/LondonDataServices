// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Text;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Decryptions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Services.Foundations.Decryptions;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Decryptions
{
    public partial class DecryptionServiceTests
    {
        private readonly Mock<IDecryptionBroker> decryptionBrokerMock;
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IDecryptionService decryptionService;

        public DecryptionServiceTests()
        {
            this.decryptionBrokerMock = new Mock<IDecryptionBroker>();
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.decryptionService = new DecryptionService(
                decryptionBroker: this.decryptionBrokerMock.Object,
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomMessage() =>
                new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        public byte[] CreateRandomDecryption()
        {
            string randomMessage = GetRandomMessage();
            return Encoding.ASCII.GetBytes(randomMessage);
        }
    }
}

