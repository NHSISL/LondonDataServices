// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.OptOuts
{
    public partial class OptOutsApiTests
    {
        [Fact]
        public async Task ShouldPostOptOutAsync()
        {
            // Given
            OptOut randomOptOut = CreateRandomOptOut();
            OptOut inputOptOut = randomOptOut;
            OptOut expectedOptOut = inputOptOut;

            // When
            OptOut actualOptOut =
                await this.apiBroker.PostOptOutAsync(inputOptOut);

            // Then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            // Cleanup
            await this.apiBroker.DeleteOptOutByIdAsync(actualOptOut.Id);
        }

        [Fact]
        public async Task ShouldGetOptOutByNhsNumberAsync()
        {
            // Given
            OptOut randomOptOut = await PostRandomOptOutAsync();
            OptOut expectedOptOut = randomOptOut;

            // When
            OptOut actualOptOut =
                await this.apiBroker.GetOptOutByNhsNumberAsync(randomOptOut.NhsNumber);

            // Then
            actualOptOut.Should().BeEquivalentTo(expectedOptOut, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            // Cleanup
            await this.apiBroker.DeleteOptOutByIdAsync(actualOptOut.Id);
        }

        [Fact]
        public async Task ShouldGetAllOptOutsAsync()
        {
            // Given
            IQueryable<OptOut> randomOptOuts = CreateRandomOptOuts();
            IQueryable<OptOut> inputOptOuts = randomOptOuts;
            IQueryable<OptOut> expectedOptOuts = inputOptOuts;

            foreach (OptOut inputOptOut in inputOptOuts)
            {
                await this.apiBroker.PostOptOutAsync(inputOptOut);
            }

            // When
            List<OptOut> actualOptOuts = await this.apiBroker.GetAllOptOutsAsync();

            // Then
            foreach (OptOut expectedOptOut in expectedOptOuts)
            {
                OptOut actualOptOut = actualOptOuts.Single(approval => approval.Id == expectedOptOut.Id);

                actualOptOut.Should().BeEquivalentTo(expectedOptOut, options => options
                    .Excluding(property => property.CreatedBy)
                    .Excluding(property => property.CreatedDate)
                    .Excluding(property => property.UpdatedBy)
                    .Excluding(property => property.UpdatedDate));

                await this.apiBroker.DeleteOptOutByIdAsync(actualOptOut.Id);
            }
        }

        [Fact]
        public async Task ShouldPutOptOutAsync()
        {
            // given
            OptOut randomOptOut = await PostRandomOptOutAsync();
            OptOut modifiedOptOut = UpdateOptOutWithRandomValues(randomOptOut);

            // when
            await this.apiBroker.PutOptOutAsync(modifiedOptOut);
            OptOut actualOptOut = await this.apiBroker.GetOptOutByNhsNumberAsync(randomOptOut.NhsNumber);

            // then
            actualOptOut.Should().BeEquivalentTo(modifiedOptOut, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            await this.apiBroker.DeleteOptOutByIdAsync(actualOptOut.Id);
        }

        [Fact]
        public async Task ShouldDeleteOptOutAsync()
        {
            // given
            OptOut randomOptOut = await PostRandomOptOutAsync();
            OptOut inputOptOut = randomOptOut;
            OptOut expectedOptOut = inputOptOut;

            // when
            OptOut deletedOptOut =
                await this.apiBroker.DeleteOptOutByIdAsync(inputOptOut.Id);

            OptOut optOutByNhsNumber =
                 await this.apiBroker.GetOptOutByNhsNumberAsync(inputOptOut.NhsNumber);

            // then
            deletedOptOut.Should().BeEquivalentTo(expectedOptOut, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            optOutByNhsNumber.Should().BeNull();
        }
    }
}