// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Mesh
{
    public interface IMeshBroker
    {
        ValueTask<bool> ValidateAccess();

        ValueTask<List<string>> CheckInboxForDownload();

        ValueTask<List<string>> GetMessageById();

        ValueTask<List<string>> AcknowledgeMessageById();

        ValueTask<List<string>> SendMessageForOptOutStatus();

        ValueTask<List<string>> GetTrackingStatus();
    }
}
