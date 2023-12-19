// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Terminologies
{
    public partial class TerminologyTests
    {
        //[Fact]
        //public async Task ShouldRetrieveArtifactMetadataAsync()
        //{
        //    //Given
        //    DateTimeOffset randomDateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();
        //    string resourceType = "CodingSystem";

        //    TerminologyPoll retrievedTerminologyPoll =
        //        CreateRandomTerminologyPoll(resourceType, lastPoll: randomDateTimeOffset.AddDays(-3));

            

        //    string relativeUrl = this.terminologyMetadataConfiguration.ResourceURL;
        //    relativeUrl = relativeUrl.Replace("{{resourceType}}", resourceType);
        //    relativeUrl = relativeUrl.Replace("{{datestamp}}", retrievedTerminologyPoll.LastPoll.ToString());

        //    this.ontologyBrokerMock.Setup(broker =>
        //        broker.GetAllCodingSystemsAsync(relativeUrl)).ReturnsAsync();


        //    //When
        //    await this.terminologyClient.RetrieveArtifactMetadataAsync(resourceType);

        //    //Then
        //    this.ontologyBrokerMock.Verify(broker =>
        //        broker.GetAllCodingSystemsAsync(relativeUrl),
        //            Times.Once);

            

        //    await this.dataSetSpecificationProcessingService
        //        .RemoveDataSetSpecificationByIdAsync(activeDataSetSpecification.Id);

        //    this.ontologyBrokerMock.VerifyNoOtherCalls();
        //}
    }
}
