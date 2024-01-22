// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Models.Foundations.Mesh
{
    public class MessageTrackingInfo
    {
        public string AddressType { get; set; } = string.Empty;
        public string Checksum { get; set; } = string.Empty;
        public int ChunkCount { get; set; }
        public string CompressFlag { get; set; } = string.Empty;
        public string DownloadTimestamp { get; set; } = string.Empty;
        public string DtsId { get; set; } = string.Empty;
        public string EncryptedFlag { get; set; } = string.Empty;
        public string ExpiryTime { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public int FileSize { get; set; }
        public string IsCompressed { get; set; } = string.Empty;
        public string LocalId { get; set; } = string.Empty;
        public string MeshRecipientOdsCode { get; set; } = string.Empty;
        public string MessageId { get; set; } = string.Empty;
        public string MessageType { get; set; } = string.Empty;
        public string PartnerId { get; set; } = string.Empty;
        public string Recipient { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
        public string RecipientOrgCode { get; set; } = string.Empty;
        public string RecipientSmtp { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderOdsCode { get; set; } = string.Empty;
        public string SenderOrgCode { get; set; } = string.Empty;
        public string SenderSmtp { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string StatusSuccess { get; set; } = string.Empty;
        public string UploadTimestamp { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string WorkflowId { get; set; } = string.Empty;
    }
}
