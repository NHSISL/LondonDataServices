// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;

namespace LHDS.Core.Models.Foundations.MeshItems
{
    internal class MeshMessage
    {
        public string MessageId { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string WorkflowId { get; set; }
        public Dictionary<string, List<string>> Headers { get; set; }
        public string ContentType { get; set; }
        public string Body { get; set; }
        public byte[] FileContent { get; set; }
        public TrackingInfo TrackingInfo { get; set; }
    }
}
