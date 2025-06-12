// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSetSpecifications
{
    public partial class DataSetSpecificationProcessingServiceTests
    {
        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public async Task ShouldRetrieveActiveDataSetSpecificationsAsync(bool isActive, bool isPublished)
        {
            // given
            Guid randomSupplierId = Guid.NewGuid();
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            DataSet randomDataSet = CreateRandomDataSet(randomSupplierId);

            List<DataSetSpecification> invalidDatesDataSetSpecifications =
                CreateRandomDataSetSpecifications(
                    dataSet: randomDataSet,
                    dataSetId: randomDataSet.Id,
                    count: 1,
                    isActive: true,
                    isPublished: true,
                    activeFrom: randomDateTimeOffset,
                    activeTo: randomDateTimeOffset);

            List<DataSetSpecification> invalidIsActiveIsPublishedDataSetSpecifications =
                CreateRandomDataSetSpecifications(
                    dataSet: randomDataSet,
                    dataSetId: randomDataSet.Id,
                    count: 1,
                    isActive: isActive,
                    isPublished: isPublished,
                    activeFrom: randomDateTimeOffset,
                    activeTo: randomDateTimeOffset.AddDays(1));

            DataSetSpecification activeDataSetSpecification =
                CreateRandomDataSetSpecifications(
                    dataSet: randomDataSet,
                    dataSetId: randomDataSet.Id,
                    count: 1,
                    isActive: true,
                    isPublished: true,
                    activeFrom: randomDateTimeOffset,
                    activeTo: randomDateTimeOffset.AddDays(1)).First();

            List<DataSetSpecification> randomDataSetSpecifications =
                [.. invalidDatesDataSetSpecifications, .. invalidIsActiveIsPublishedDataSetSpecifications];

            randomDataSetSpecifications.Add(activeDataSetSpecification);

            IQueryable<DataSetSpecification> storageDataSetSpecifications = randomDataSetSpecifications.AsQueryable();
            DataSetSpecification expectedDataSetSpecification = activeDataSetSpecification.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.dataSetSpecificationServiceMock.Setup(broker =>
                broker.RetrieveAllDataSetSpecificationsAsync())
                    .ReturnsAsync(storageDataSetSpecifications);

            // when
            DataSetSpecification actualDataSetSpecification =
                 await this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(randomSupplierId);

            // then
            actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.dataSetSpecificationServiceMock.Verify(broker =>
                broker.RetrieveAllDataSetSpecificationsAsync(),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
