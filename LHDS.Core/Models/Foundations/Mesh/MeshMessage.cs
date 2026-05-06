// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;

namespace LHDS.Core.Models.Foundations.Mesh
{
    public class MeshMessage
    {
        public string MessageId { get; set; } = string.Empty;
        public Dictionary<string, List<string>> Headers { get; set; } = new Dictionary<string, List<string>>();
        public MessageTrackingInfo TrackingInfo { get; set; } = null!;
    }
}
