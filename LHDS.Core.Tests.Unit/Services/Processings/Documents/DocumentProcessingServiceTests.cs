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
using LHDS.Core.Models.Foundations.Audits.Exceptions;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using Xunit;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;

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
                this.documentServiceMock.Object,
                this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
           actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        public static TheoryData DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentValidationException(innerException),
                new DocumentDependencyValidationException(innerException)
            };
        }

        public static TheoryData DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentDependencyException(innerException),
                new DocumentServiceException(innerException)
            };
        }

        private string GetValidationSummary(IDictionary data)
        {
            StringBuilder validationSummary = new StringBuilder();

            foreach (DictionaryEntry entry in data)
            {
                string errorSummary = ((List<string>)entry.Value)
                    .Select((string value) => value)
                    .Aggregate((string current, string next) => current + ", " + next);

                validationSummary.Append($"{entry.Key} => {errorSummary};  ");
            }

            return validationSummary.ToString();
        }
    }
}
