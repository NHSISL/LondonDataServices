// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Terminologies
{
    public partial class TerminologyTests
    {
        [Fact]
        public async System.Threading.Tasks.Task ShouldRetrieveArtifactMetadataAsync()
        {
            //Given
            DateTimeOffset randomDateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();
            string resourceType = "CodeSystem";
            string nextPageUrl = "";
            List<dynamic> randomArtifactProperties = CreateRandomArtifactProperties(resourceType);
            Bundle randomCodingSystemBundle = CreateCodeSystemBundleFromRandomData(randomArtifactProperties, nextPageUrl);

            this.ontologyBrokerMock.Setup(broker =>
                broker.GetAllCodingSystemsAsync(It.IsAny<string>()))
                    .ReturnsAsync(randomCodingSystemBundle);

            //When
            await this.terminologyClient.RetrieveArtifactMetadataAsync(resourceType);

            //Then
            this.ontologyBrokerMock.Verify(broker =>
                broker.GetAllCodingSystemsAsync(It.IsAny<string>()),
                    Times.Once);

            IQueryable<TerminologyArtifact> terminologyArtifacts =
                this.terminologyArtifactService.RetrieveAllTerminologyArtifacts();

            foreach (TerminologyArtifact terminologyArtifact in terminologyArtifacts)
            {
                await this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(terminologyArtifact.Id);
            }

            this.ontologyBrokerMock.VerifyNoOtherCalls();
        }
    }
}
