// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.Mesh
{
    public class MeshBroker : IMeshBroker
    {
        public ValueTask<List<string>> AcknowledgeMessageById() =>
            throw new System.NotImplementedException();

        public ValueTask<List<string>> CheckInboxForDownload() =>
            throw new System.NotImplementedException();

        public ValueTask<List<string>> GetMessageById() =>
            throw new System.NotImplementedException();

        public ValueTask<List<string>> GetTrackingStatus() =>
            throw new System.NotImplementedException();

        public ValueTask<List<string>> SendMessageForOptOutStatus() =>
            throw new System.NotImplementedException();

        public ValueTask<bool> ValidateAccess() =>
            throw new System.NotImplementedException();
    }
}
