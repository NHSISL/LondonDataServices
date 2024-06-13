// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;

namespace LHDS.Core.Models.Brokers.Mesh
{
    public class MeshConfigurationSettings
    {
        public string MailboxId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string RootCertificate { get; set; } = string.Empty;
        public List<string> IntermediateCertificates { get; set; } = new List<string>();
        public string ClientCertificate { get; set; } = string.Empty;
        public string MexClientVersion { get; set; } = string.Empty;
        public string MexOSName { get; set; } = string.Empty;
        public string MexOSVersion { get; set; } = string.Empty;
        public int MaxChunkSizeInMegabytes { get; set; }
    }
}
