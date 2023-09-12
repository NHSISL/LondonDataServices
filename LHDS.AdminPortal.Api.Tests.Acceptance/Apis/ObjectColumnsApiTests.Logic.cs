// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.ObjectColumns;
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
            await this.apiBroker.DeleteObjectColumnByIdAsync(actualObjectColumn.Id);
            await CleanupTask(actualObjectColumn);
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
                await this.apiBroker.DeleteObjectColumnByIdAsync(expectedObjectColumn.Id);
            }

            ObjectColumn firstObjectColumn = expectedObjectColumns.First();
            await CleanupTask(firstObjectColumn);
        }
    }
}