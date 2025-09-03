// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.SubscriberPractices
{
    public partial class SubscriberPracticesApiTests
    {
        [Fact]
        public async Task ShouldPostSubscriberPracticeAsync()
        {
            // given
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice();
            SubscriberPractice inputSubscriberPractice = randomSubscriberPractice;
            SubscriberPractice expectedSubscriberPractice = inputSubscriberPractice;

            // when 
            await this.apiBroker.PostSubscriberPracticeAsync(inputSubscriberPractice);

            SubscriberPractice actualSubscriberPractice =
                await this.apiBroker.GetSubscriberPracticeByIdAsync(inputSubscriberPractice.Id);

            // then
            actualSubscriberPractice.Should().BeEquivalentTo(expectedSubscriberPractice, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            await this.apiBroker.DeleteSubscriberPracticeByIdAsync(actualSubscriberPractice.Id);
        }

        [Fact]
        public async Task ShouldGetAllSubscriberPracticesAsync()
        {
            // given
            List<SubscriberPractice> randomSubscriberPractices = await PostRandomSubscriberPracticesAsync();
            List<SubscriberPractice> expectedSubscriberPractices = randomSubscriberPractices;

            // when
            List<SubscriberPractice> actualSubscriberPractices =
                await this.apiBroker.GetAllSubscriberPracticesAsync();

            // then
            foreach (SubscriberPractice expectedSubscriberPractice in expectedSubscriberPractices)
            {
                SubscriberPractice actualSubscriberPractice =
                    actualSubscriberPractices.Single(approval => approval.Id == expectedSubscriberPractice.Id);

                actualSubscriberPractice.Should().BeEquivalentTo(expectedSubscriberPractice, options => options
                    .Excluding(property => property.CreatedBy)
                    .Excluding(property => property.CreatedDate)
                    .Excluding(property => property.UpdatedBy)
                    .Excluding(property => property.UpdatedDate));

                await this.apiBroker.DeleteSubscriberPracticeByIdAsync(actualSubscriberPractice.Id);
            }
        }

        [Fact]
        public async Task ShouldGetSubscriberPracticeAsync()
        {
            // given
            SubscriberPractice randomSubscriberPractice = await PostRandomSubscriberPracticeAsync();
            SubscriberPractice expectedSubscriberPractice = randomSubscriberPractice;

            // when
            SubscriberPractice actualSubscriberPractice =
                await this.apiBroker.GetSubscriberPracticeByIdAsync(randomSubscriberPractice.Id);

            // then
            actualSubscriberPractice.Should().BeEquivalentTo(expectedSubscriberPractice, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            await this.apiBroker.DeleteSubscriberPracticeByIdAsync(actualSubscriberPractice.Id);
        }

        [Fact]
        public async Task ShouldPutSubscriberPracticeAsync()
        {
            // given
            SubscriberPractice randomSubscriberPractice = await PostRandomSubscriberPracticeAsync();

            SubscriberPractice modifiedSubscriberPractice =
                UpdateSubscriberPracticeWithRandomValues(randomSubscriberPractice);

            // when
            await this.apiBroker.PutSubscriberPracticeAsync(modifiedSubscriberPractice);

            SubscriberPractice actualSubscriberPractice =
                await this.apiBroker.GetSubscriberPracticeByIdAsync(randomSubscriberPractice.Id);

            // then
            actualSubscriberPractice.Should().BeEquivalentTo(modifiedSubscriberPractice, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));
            await this.apiBroker.DeleteSubscriberPracticeByIdAsync(actualSubscriberPractice.Id);
        }

        [Fact]
        public async Task ShouldDeleteSubscriberPracticeAsync()
        {
            // given
            SubscriberPractice randomSubscriberPractice = await PostRandomSubscriberPracticeAsync();
            SubscriberPractice inputSubscriberPractice = randomSubscriberPractice;
            SubscriberPractice expectedSubscriberPractice = inputSubscriberPractice;

            // when
            SubscriberPractice deletedSubscriberPractice =
                await this.apiBroker.DeleteSubscriberPracticeByIdAsync(inputSubscriberPractice.Id);

            ValueTask<SubscriberPractice> getSubscriberPracticebyIdTask =
                this.apiBroker.GetSubscriberPracticeByIdAsync(inputSubscriberPractice.Id);

            // then
            deletedSubscriberPractice.Should().BeEquivalentTo(expectedSubscriberPractice, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(getSubscriberPracticebyIdTask.AsTask);
        }
    }
}