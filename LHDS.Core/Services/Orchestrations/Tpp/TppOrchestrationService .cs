// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.Core.Services.Orchestrations.Tpp
{
    public partial class TppOrchestrationService : ITppOrchestrationService
    {
        public async ValueTask<string> ProcessAsync(Document fileName) =>
            throw new System.NotImplementedException();
    }
}