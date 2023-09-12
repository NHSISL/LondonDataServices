// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.ObjectColumns;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SpecificationObjects;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.ObjectColumns
{
    public partial class ObjectColumnsApiTests
    {
        [Fact]
        public async Task ShouldPostSpecificationObjectAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            await this.apiBroker.PostDataSetAsync(randomDataSet);

            DataSetSpecification randomDataSetSpecification = 
                CreateRandomDataSetSpecification(dataSetId: randomDataSet.Id);

            await this.apiBroker.PostDataSetSpecificationAsync(randomDataSetSpecification);

            SpecificationObject randomSpecificationObject = 
                CreateRandomSpecificationObject(dataSetSpecificationId: randomDataSetSpecification.Id);

            await this.apiBroker.PostSpecificationObjectAsync(randomSpecificationObject);

            ObjectColumn randomObjectColumn =
                CreateRandomObjectColumn(specificationObjectId: randomSpecificationObject.Id);

            ObjectColumn inputObjectColumn = randomObjectColumn;
            ObjectColumn expectedObjectColumn = inputObjectColumn;

            // When
            ObjectColumn actualObjectColumn =
                await this.apiBroker.PostObjectColumnAsync(inputObjectColumn);

            // Then
            actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);

            // Cleanup
            await this.apiBroker.DeleteObjectColumnByIdAsync(actualObjectColumn.Id);
            await this.apiBroker.DeleteSpecificationObjectByIdAsync(randomSpecificationObject.Id);
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
        }
    }
}