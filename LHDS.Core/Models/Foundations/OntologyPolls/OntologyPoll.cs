// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;

namespace LHDS.Core.Models.Foundations.OntologyPolls
{
    internal class OntologyPoll : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string ArtifactName { get; set; }
        public DateTimeOffset? LastPoll { get; set; }
        public bool IsDownloaded { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
