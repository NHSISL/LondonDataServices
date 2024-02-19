// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Orchestrations.Decryptions;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decryptions
{
    public partial class DecryptionCoordinationServiceTests
    {
        private readonly Mock<ISubscriberCredentialOrchestration> subscriberCredentialOrchestrationMock;
        private readonly Mock<IDecryptionOrchestrationService> decryptionOrchestrationServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IDecryptionCoordinationService decryptionCoordinationService;

        public DecryptionCoordinationServiceTests()
        {
            this.subscriberCredentialOrchestrationMock = new Mock<ISubscriberCredentialOrchestration>();
            this.decryptionOrchestrationServiceMock = new Mock<IDecryptionOrchestrationService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.decryptionCoordinationService = new DecryptionCoordinationService(
                subscriberCredentialOrchestration: subscriberCredentialOrchestrationMock.Object,
                decryptionOrchestrationService: decryptionOrchestrationServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}
