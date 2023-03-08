// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------


using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Processings.Documents;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Processings.Documents
{
    public partial class DocumentProcessingServiceTests
    {
        private readonly Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
        private readonly IDocumentProcessingService documentProcessingService;

        public DocumentProcessingServiceTests()
        {
            documentProcessingService = new DocumentProcessingService(documentServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
    }
}
