// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using LHDS.Core.Clients;
using LHDS.Core.Models.Orchestrations.AddressToUprns.Exceptions;
using LHDS.Core.Services.Orchestrations.AddressToUprns;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Clients.AddressToUprnClients
{
    public partial class AddressToUprnClientTests
    {
        private readonly Mock<IAddressToUprnOrchestrationService> addressToUprnOrchestrationServiceMock;
        private readonly IAddressToUprnClient addressToUprnClient;

        public AddressToUprnClientTests()
        {
            this.addressToUprnOrchestrationServiceMock = new Mock<IAddressToUprnOrchestrationService>();

            this.addressToUprnClient = new AddressToUprnClient(
                addressToUprnOrchestrationService: this.addressToUprnOrchestrationServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static Guid GetRandomGuid() =>
            Guid.NewGuid();

        private static Stream CreateRandomStream()
        {
            string content = GetRandomString();
            byte[] bytes = Encoding.UTF8.GetBytes(content);

            return new MemoryStream(bytes);
        }

        public static TheoryData<Xeption> AddressToUprnClientDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            var innerException = new Xeption(randomMessage);

            return new TheoryData<Xeption>
            {
                new AddressToUprnOrchestrationValidationException(
                    message: "Address to UPRN orchestration validation error occurred, fix the errors and try again.",
                    innerException),

                new AddressToUprnOrchestrationDependencyValidationException(
                    message: "Address to UPRN orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException)
            };
        }

        public static TheoryData<Xeption> AddressToUprnClientDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            var innerException = new Xeption(randomMessage);

            return new TheoryData<Xeption>
            {
                new AddressToUprnOrchestrationDependencyException(
                    message: "Address to UPRN orchestration dependency error occurred, fix the errors and try again.",
                    innerException)
            };
        }

        public static TheoryData<Xeption> AddressToUprnClientServiceExceptions()
        {
            string randomMessage = GetRandomString();
            var innerException = new Xeption(randomMessage);

            return new TheoryData<Xeption>
            {
                new AddressToUprnOrchestrationServiceException(
                    message: "Address to UPRN orchestration service error occurred, fix the errors and try again.",
                    innerException)
            };
        }
    }
}
