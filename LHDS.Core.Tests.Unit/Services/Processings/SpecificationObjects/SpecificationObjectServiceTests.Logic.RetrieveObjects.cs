// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SpecificationObjects
{
    public partial class SpecificationObjectProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveSpecificationObjectsByDataSetSpecificationIdAsync()
        {
            // Given
            Guid randomDataSetSpecificationId = Guid.NewGuid();

            List<SpecificationObject> randomSpecificationObjects =
                CreateRandomSpecificationObjects(dataSetSpecificationId: randomDataSetSpecificationId);

            List<SpecificationObject> storageSpecificationObjects = randomSpecificationObjects;

            List<string> expectedSpecificationObjects = storageSpecificationObjects
                .Select(specificationObject => specificationObject.SupplierObjectName)
                .ToList();

            this.specificationObjectServiceMock.Setup(service =>
                service.RetrieveAllSpecificationObjects())
                    .Returns(storageSpecificationObjects.AsQueryable());

            // When
            List<string> actualSpecificationObjects =
                await this.specificationObjectProcessingService
                    .RetrieveSpecificationObjectsByDataSetSpecificationId(randomDataSetSpecificationId);

            // Then
            actualSpecificationObjects.Should().BeEquivalentTo(expectedSpecificationObjects);

            this.specificationObjectServiceMock.Verify(service =>
                service.RetrieveAllSpecificationObjects(),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
