// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;

namespace LHDS.Core.Models.Foundations.Mesh
{
    public class MeshMessage
    {
        public string MessageId { get; set; }
        public Dictionary<string, List<string>> Headers { get; set; } = new Dictionary<string, List<string>>();
        public string StringContent { get; set; }
        public byte[] FileContent { get; set; }
        public MessageTrackingInfo TrackingInfo { get; set; }
    }
}
