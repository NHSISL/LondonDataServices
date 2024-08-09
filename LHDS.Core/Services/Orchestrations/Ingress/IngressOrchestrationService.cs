// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.Ingress
{
    public class IngressOrchestrationService : IIngressOrchestrationService
    {
        public ValueTask CheckForEmisBatchCompleteAsync(string encryptedFileName) =>
            ValueTask.CompletedTask;

        public ValueTask CheckForTPPBatchCompleteAsync(string fileName) =>
            ValueTask.CompletedTask;
    }
}
