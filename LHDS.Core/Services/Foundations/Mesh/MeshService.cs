// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Mesh;

namespace LHDS.Core.Services.Foundations.Mesh
{
    public partial class MeshService : IMeshService
    {
        private readonly IMeshBroker meshBroker;
        private readonly ILoggingBroker loggingBroker;

        public MeshService(
            IMeshBroker meshBroker,
            ILoggingBroker loggingBroker)
        {
            this.meshBroker = meshBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<bool> ValidateAccessAsync()
        {
            return this.meshBroker.ValidateAccessAsync();
        }
    }
}
