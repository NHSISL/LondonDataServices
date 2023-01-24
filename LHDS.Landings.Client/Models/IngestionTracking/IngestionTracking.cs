// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHDS.Landings.Client.Models.IngestionTracking
{
    public class IngestionTracking
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid EncryptedBlobId { get; set; }
        public Guid DecryptedBlobId { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdaterByUserId { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
