// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using NEL.Premises.Api.Models.Bases;

namespace LHDS.Landings.Client.Models.Foundations.IngestionTrackings
{
    public class IngestionTracking : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid EncryptedBlobId { get; set; }
        public Guid DecryptedBlobId { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
