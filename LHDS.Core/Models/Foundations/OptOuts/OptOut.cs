// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;

namespace LHDS.Core.Models.Foundations.OptOuts
{
    public class OptOut : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string NhsNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string BatchReference { get; set; } = string.Empty;
        public string UniqueReference { get; set; } = string.Empty;
        public DateTimeOffset CacheTime { get; set; }
        public DateTimeOffset LastSentToMesh { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
