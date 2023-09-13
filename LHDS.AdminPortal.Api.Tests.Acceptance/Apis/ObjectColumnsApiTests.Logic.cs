// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.ObjectColumns;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.ObjectColumns
{
    public partial class ObjectColumnsApiTests
    {
        [Fact]
        public async Task ShouldPostObjectColumnAsync()
        {
            // Given
            ObjectColumn randomObjectColumn = await CreateRandomObjectColumnAsync();
            ObjectColumn inputObjectColumn = randomObjectColumn;
            ObjectColumn expectedObjectColumn = inputObjectColumn;

            // When
            ObjectColumn actualObjectColumn =
                await this.apiBroker.PostObjectColumnAsync(inputObjectColumn);

            // Then
            actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);

            // Cleanup
            await CleanupTask(actualObjectColumn, false);
        }

        [Fact]
        public async Task ShouldGetAllObjectColumnsAsync()
        {
            // Given
            List<ObjectColumn> randomObjectColumns = await CreateRandomObjectColumns();
            List<ObjectColumn> inputObjectColumns = randomObjectColumns;
            List<ObjectColumn> expectedObjectColumns = inputObjectColumns;

            foreach (ObjectColumn inputObjectColumn in inputObjectColumns)
            {
                await this.apiBroker.PostObjectColumnAsync(inputObjectColumn);
            }

            // When
            List<ObjectColumn> actualObjectColumns =
                await this.apiBroker.GetAllObjectColumnsAsync();

            // Then
            foreach (ObjectColumn expectedObjectColumn in expectedObjectColumns)
            {
                ObjectColumn actualObjectColumn =
                    actualObjectColumns.Single(approval => approval.Id == expectedObjectColumn.Id);

                actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);
                await CleanupTask(actualObjectColumn, false);
            }
        }

        [Fact]
        public async Task ShouldGetObjectColumnByIdAsync()
        {
            // Given
            ObjectColumn randomObjectColumn = await CreateRandomObjectColumnAsync();
            ObjectColumn inputObjectColumn = randomObjectColumn;
            ObjectColumn expectedObjectColumn = inputObjectColumn;
            await this.apiBroker.PostObjectColumnAsync(inputObjectColumn);

            // When
            ObjectColumn actualObjectColumn =
                await this.apiBroker.GetObjectColumnByIdAsync(inputObjectColumn.Id);

            // Then
            actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);

            // Cleanup
            await CleanupTask(actualObjectColumn, false);
        }

        [Fact]
        public async Task ShouldPutObjectColumnAsync()
        {
            // Given
            ObjectColumn randomObjectColumn = await CreateRandomObjectColumnAsync();
            ObjectColumn inputObjectColumn = randomObjectColumn;
            await this.apiBroker.PostObjectColumnAsync(inputObjectColumn);

            ObjectColumn modifiedObjectColumn =
                UpdateObjectColumnWithRandomValues(inputObjectColumn);

            // When
            ObjectColumn actualObjectColumn =
                await this.apiBroker.PutObjectColumnAsync(modifiedObjectColumn);

            // Then
            actualObjectColumn.Should().BeEquivalentTo(modifiedObjectColumn);

            // Cleanup
            await CleanupTask(actualObjectColumn, false);
        }

        [Fact]
        public async Task ShouldDeleteSpecificationObjectAsync()
        {
            // Given
            ObjectColumn randomObjectColumn = await CreateRandomObjectColumnAsync();
            ObjectColumn inputObjectColumn = randomObjectColumn;
            ObjectColumn expectedObjectColumn = inputObjectColumn;
            await this.apiBroker.PostObjectColumnAsync(inputObjectColumn);

            // when
            ObjectColumn deletedObjectColumn =
                await this.apiBroker.DeleteObjectColumnByIdAsync(inputObjectColumn.Id);

            ValueTask<ObjectColumn> getObjectColumnbyIdTask =
                this.apiBroker.GetObjectColumnByIdAsync(inputObjectColumn.Id);

            // then
            deletedObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
                getObjectColumnbyIdTask.AsTask());

            // Cleanup
            await CleanupTask(expectedObjectColumn, true);
        }
    }
}