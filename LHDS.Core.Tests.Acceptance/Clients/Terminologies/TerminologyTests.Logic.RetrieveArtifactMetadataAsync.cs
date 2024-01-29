// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Terminologies
{
    public partial class TerminologyTests
    {
        [Theory]
        [InlineData("CodeSystem")]
        [InlineData("ValueSet")]
        [InlineData("ConceptMap")]
        public async System.Threading.Tasks.Task ShouldRetrievePagedArtifactMetadataAsync(string inputResourceType)
        {
            //Given
            string resourceType = inputResourceType;
            string nextPageUrl = "";
            List<dynamic> randomArtifactProperties = CreateRandomArtifactProperties(resourceType);

            Bundle randomCodeSystemBundle = 
                CreateCodeSystemBundleFromRandomData(randomArtifactProperties, nextPageUrl);

            Bundle randomConceptMapBundle =
                CreateConceptMapBundleFromRandomData(randomArtifactProperties, nextPageUrl);

            Bundle randomValueSetBundle =
                CreateValueSetBundleFromRandomData(randomArtifactProperties, nextPageUrl);

            this.ontologyBrokerMock.Setup(broker =>
                broker.GetAllCodingSystemsAsync(It.IsAny<string>()))
                    .ReturnsAsync(randomCodeSystemBundle);

            this.ontologyBrokerMock.Setup(broker =>
                broker.GetAllValueSetsAsync(It.IsAny<string>()))
                    .ReturnsAsync(randomValueSetBundle);

            this.ontologyBrokerMock.Setup(broker =>
                broker.GetAllConceptMapsAsync(It.IsAny<string>()))
                    .ReturnsAsync(randomConceptMapBundle);

            //When
            await this.terminologyClient.RetrieveArtifactMetadataAsync(resourceType);

            //Then
            if(resourceType == "CodeSystem")
            {
                this.ontologyBrokerMock.Verify(broker =>
                    broker.GetAllCodingSystemsAsync(It.IsAny<string>()),
                        Times.Once);
            }

            if (resourceType == "ValueSet")
            {
                this.ontologyBrokerMock.Verify(broker =>
                    broker.GetAllValueSetsAsync(It.IsAny<string>()),
                        Times.Once);
            }

            if (resourceType == "ConceptMap")
            {
                this.ontologyBrokerMock.Verify(broker =>
                    broker.GetAllConceptMapsAsync(It.IsAny<string>()),
                        Times.Once);
            }

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
