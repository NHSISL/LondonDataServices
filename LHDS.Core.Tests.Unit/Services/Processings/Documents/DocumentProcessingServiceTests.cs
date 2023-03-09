// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq.Expressions;
using System;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Processings.Documents;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Processings.Documents
{
    public partial class DocumentProcessingServiceTests
    {
        private readonly Mock<IDocumentService> documentServiceMock = new Mock<IDocumentService>();
        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IDocumentProcessingService documentProcessingService;

        public DocumentProcessingServiceTests()
        {
            this.documentProcessingService = new DocumentProcessingService(
                this.documentServiceMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
           actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
    }
}
