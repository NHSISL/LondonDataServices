// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Services.Foundations.Mesh
{
    public interface IMeshService
    {
        ValueTask<bool> ValidateAccessAsync();

        ValueTask<bool> AcknowledgeMessageByIdAsync(string inputMailboxId, string inputMessageId);
    }
}
