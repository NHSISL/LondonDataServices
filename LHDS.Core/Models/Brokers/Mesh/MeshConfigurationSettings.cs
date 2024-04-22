// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;

namespace LHDS.Core.Models.Brokers.Mesh
{
    public class MeshConfigurationSettings
    {
        public string MailboxId { get; set; }
        public string Password { get; set; }
        public string Key { get; set; }
        public string Url { get; set; }
        public string RootCertificate { get; set; }
        public List<string> IntermediateCertificates { get; set; }
        public string ClientCertificate { get; set; }
        public string MexClientVersion { get; set; }
        public string MexOSName { get; set; }
        public string MexOSVersion { get; set; }
        public int MaxChunkSizeInMegabytes { get; set; }
    }
}
