// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Services.Orchestrations.OptOuts;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Mesh;
using LHDS.Core.Services.Processings.OptOuts;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        private readonly Mock<IOptOutProcessingService> optOutProcessingServiceMock;
        private readonly Mock<IDocumentProcessingService> documentProcessingServiceMock;
        private readonly Mock<IMeshProcessingService> meshProcessingServiceMock;
        private readonly OptOutOrchestrationService optOutOrchestrationService;

        public OptOutOrchestrationTests()
        {
            this.optOutProcessingServiceMock = new Mock<IOptOutProcessingService>();
            this.documentProcessingServiceMock = new Mock<IDocumentProcessingService>();
            this.meshProcessingServiceMock = new Mock<IMeshProcessingService>();

            this.optOutOrchestrationService = new OptOutOrchestrationService(
                optOutProcessingService: optOutProcessingServiceMock.Object,
                documentProcessingService: documentProcessingServiceMock.Object,
                meshProcessingService: meshProcessingServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
    }
}
