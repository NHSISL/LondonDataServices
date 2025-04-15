// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.TerminologyPolls;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.TerminologyPolls
{
    public partial class TerminologyPollsApiTests
    {
        [Fact]
        public async Task ShouldPostTerminologyPollAsync()
        {
            // given
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            TerminologyPoll inputTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = inputTerminologyPoll;

            // when 
            await this.apiBroker.PostTerminologyPollAsync(inputTerminologyPoll);

            TerminologyPoll actualTerminologyPoll =
                await this.apiBroker.GetTerminologyPollByIdAsync(inputTerminologyPoll.Id);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            await this.apiBroker.DeleteTerminologyPollByIdAsync(actualTerminologyPoll.Id);
        }

        [Fact]
        public async Task ShouldGetAllTerminologyPollsAsync()
        {
            // given
            List<TerminologyPoll> randomTerminologyPolls = await PostRandomTerminologyPollsAsync();
            List<TerminologyPoll> expectedTerminologyPolls = randomTerminologyPolls;

            // when
            List<TerminologyPoll> actualTerminologyPolls = await this.apiBroker.GetAllTerminologyPollsAsync();

            // then
            foreach (TerminologyPoll expectedTerminologyPoll in expectedTerminologyPolls)
            {
                TerminologyPoll actualTerminologyPoll = actualTerminologyPolls.Single(approval => approval.Id == expectedTerminologyPoll.Id);
                
                actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

                await this.apiBroker.DeleteTerminologyPollByIdAsync(actualTerminologyPoll.Id);
            }
        }

        [Fact]
        public async Task ShouldGetTerminologyPollAsync()
        {
            // given
            TerminologyPoll randomTerminologyPoll = await PostRandomTerminologyPollAsync();
            TerminologyPoll expectedTerminologyPoll = randomTerminologyPoll;

            // when
            TerminologyPoll actualTerminologyPoll = await this.apiBroker.GetTerminologyPollByIdAsync(randomTerminologyPoll.Id);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            await this.apiBroker.DeleteTerminologyPollByIdAsync(actualTerminologyPoll.Id);
        }

        [Fact]
        public async Task ShouldPutTerminologyPollAsync()
        {
            // given
            TerminologyPoll randomTerminologyPoll = await PostRandomTerminologyPollAsync();
            TerminologyPoll modifiedTerminologyPoll = UpdateTerminologyPollWithRandomValues(randomTerminologyPoll);

            // when
            await this.apiBroker.PutTerminologyPollAsync(modifiedTerminologyPoll);
            TerminologyPoll actualTerminologyPoll = await this.apiBroker.GetTerminologyPollByIdAsync(randomTerminologyPoll.Id);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(modifiedTerminologyPoll, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            await this.apiBroker.DeleteTerminologyPollByIdAsync(actualTerminologyPoll.Id);
        }

        [Fact]
        public async Task ShouldDeleteTerminologyPollAsync()
        {
            // given
            TerminologyPoll randomTerminologyPoll = await PostRandomTerminologyPollAsync();
            TerminologyPoll inputTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = inputTerminologyPoll;

            // when
            TerminologyPoll deletedTerminologyPoll =
                await this.apiBroker.DeleteTerminologyPollByIdAsync(inputTerminologyPoll.Id);

            ValueTask<TerminologyPoll> getTerminologyPollbyIdTask =
                this.apiBroker.GetTerminologyPollByIdAsync(inputTerminologyPoll.Id);

            // then
            deletedTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(getTerminologyPollbyIdTask.AsTask);
        }
    }
}