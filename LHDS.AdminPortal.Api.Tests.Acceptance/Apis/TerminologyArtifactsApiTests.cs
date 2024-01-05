using System;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.TerminologyArtifacts;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.TerminologyArtifacts
{
    [Collection(nameof(ApiTestCollection))]
    public partial class TerminologyArtifactsApiTests
    {
        private readonly ApiBroker apiBroker;

        public TerminologyArtifactsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static TerminologyArtifact CreateRandomTerminologyArtifact() =>
            CreateRandomTerminologyArtifactFiller().Create();

        private static Filler<TerminologyArtifact> CreateRandomTerminologyArtifactFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<TerminologyArtifact>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(terminologyArtifact => terminologyArtifact.CreatedDate).Use(now)
                .OnProperty(terminologyArtifact => terminologyArtifact.CreatedBy).Use(user)
                .OnProperty(terminologyArtifact => terminologyArtifact.UpdatedDate).Use(now)
                .OnProperty(terminologyArtifact => terminologyArtifact.UpdatedBy).Use(user);

            return filler;
        }
    }
}