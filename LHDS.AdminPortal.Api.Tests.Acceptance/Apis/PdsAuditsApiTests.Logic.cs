// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.PdsAudits;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.PdsAudits
{
    public partial class PdsAuditsApiTests
    {
        [Fact]
        public async Task ShouldPostPdsAuditAsync()
        {
            // Given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            PdsAudit inputPdsAudit = randomPdsAudit;
            PdsAudit expectedPdsAudit = inputPdsAudit;

            // When
            PdsAudit actualPdsAudit = await this.apiBroker.PostPdsAuditAsync(inputPdsAudit);

            // Then
            actualPdsAudit.Should().BeEquivalentTo(expectedPdsAudit, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            // Cleanup
            await this.apiBroker.DeletePdsAuditByIdAsync(inputPdsAudit.Id);
        }

        [Fact]
        public async Task ShouldGetAllPdsAuditsAsync()
        {
            // Given
            IQueryable<PdsAudit> randomPdsAudits = CreateRandomPdsAudits().AsQueryable();
            IQueryable<PdsAudit> inputPdsAudits = randomPdsAudits;
            IQueryable<PdsAudit> expectedPdsAudits = inputPdsAudits;

            foreach (PdsAudit inputPdsAudit in inputPdsAudits)
            {
                await this.apiBroker.PostPdsAuditAsync(inputPdsAudit);
            }

            // When
            List<PdsAudit> actualPdsAudits = await this.apiBroker.GetAllPdsAuditsAsync();

            // Then
            foreach (PdsAudit expectedPdsAudit in expectedPdsAudits)
            {
                PdsAudit actualPdsAudit = actualPdsAudits.Single(approval => approval.Id == expectedPdsAudit.Id);

                actualPdsAudit.Should().BeEquivalentTo(expectedPdsAudit, options => options
                    .Excluding(property => property.CreatedBy)
                    .Excluding(property => property.CreatedDate)
                    .Excluding(property => property.UpdatedBy)
                    .Excluding(property => property.UpdatedDate));

                await this.apiBroker.DeletePdsAuditByIdAsync(expectedPdsAudit.Id);
            }
        }

        [Fact]
        public async Task ShouldGetPdsAuditByIdAsync()
        {
            // Given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            PdsAudit inputPdsAudit = randomPdsAudit;
            PdsAudit expectedPdsAudit = inputPdsAudit;
            await this.apiBroker.PostPdsAuditAsync(inputPdsAudit);

            // When
            PdsAudit actualPdsAudit =
                await this.apiBroker.GetPdsAuditByIdAsync(inputPdsAudit.Id);

            // Then
            actualPdsAudit.Should().BeEquivalentTo(expectedPdsAudit, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            // Cleanup
            await this.apiBroker.DeletePdsAuditByIdAsync(inputPdsAudit.Id);
        }

        [Fact]
        public async Task ShouldDeletePdsAuditAsync()
        {
            // given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            PdsAudit inputPdsAudit = randomPdsAudit;
            PdsAudit expectedPdsAudit = inputPdsAudit;
            await this.apiBroker.PostPdsAuditAsync(inputPdsAudit);

            // when
            PdsAudit deletedPdsAudit =
                await this.apiBroker.DeletePdsAuditByIdAsync(inputPdsAudit.Id);

            ValueTask<PdsAudit> getPdsAuditbyIdTask =
                this.apiBroker.GetPdsAuditByIdAsync(inputPdsAudit.Id);

            // then
            deletedPdsAudit.Should().BeEquivalentTo(expectedPdsAudit, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(getPdsAuditbyIdTask.AsTask);
        }

        [Fact]
        public async Task ShouldPutPdsAuditAsync()
        {
            // Given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            PdsAudit inputPdsAudit = randomPdsAudit;
            PdsAudit storagePdsAudit = await this.apiBroker.PostPdsAuditAsync(inputPdsAudit);
            PdsAudit modifiedPdsAudit = UpdatePdsAuditWithRandomValues(storagePdsAudit);

            // When
            PdsAudit actualPdsAudit =
                await this.apiBroker.PutPdsAuditAsync(modifiedPdsAudit);

            // Then
            actualPdsAudit.Should().BeEquivalentTo(modifiedPdsAudit, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));
        }
    }
}