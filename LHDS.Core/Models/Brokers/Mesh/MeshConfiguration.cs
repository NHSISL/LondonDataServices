// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Security.Cryptography.X509Certificates;

namespace LHDS.Core.Models.Brokers.Mesh
{
    public class MeshConfiguration
    {
        public string MailboxId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SharedKey { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public X509Certificate2Collection TlsRootCertificates { get; set; } = new X509Certificate2Collection();
        public X509Certificate2Collection TlsIntermediateCertificates { get; set; } = new X509Certificate2Collection();
        public X509Certificate2? ClientSigningCertificate { get; set; }
        public string MexClientVersion { get; set; } = string.Empty;
        public string MexOSName { get; set; } = string.Empty;
        public string MexOSVersion { get; set; } = string.Empty;
        public int MaxChunkSizeInMegabytes { get; set; }
    }
}
