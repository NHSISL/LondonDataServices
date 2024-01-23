using System.Linq;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.Addresses;
using Xunit;
using System.Collections.Generic;
using Force.DeepCloner;
using System.Threading.Tasks;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldReturnAddressesByPostCode()
        {
            // given
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            string randomString = GetRandomString();
            string inputPostCode = randomString;

            foreach (var address in randomAddresses)
            {
                address.PostCode = inputPostCode;
            }

            List<Address> expectedAddresses = randomAddresses.DeepClone();
            List<Address> extraRandomAddresses = CreateRandomAddresses().ToList();
            List<Address> allRandomAddresses = new List<Address>();
            allRandomAddresses.AddRange(randomAddresses);
            allRandomAddresses.AddRange(extraRandomAddresses);

            IQueryable<Address> storageAddresses = allRandomAddresses.AsQueryable();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddresses())
                    .Returns(storageAddresses);

            // when
            List<Address> actualAddresses = await this.addressService.RetrieveAddressesByPostCodeAsync(inputPostCode);

            // then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddresses(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}