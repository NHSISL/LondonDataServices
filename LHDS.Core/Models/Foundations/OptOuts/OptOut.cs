// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;

namespace LHDS.Core.Models.Foundations.OptOuts
{
    public class OptOut : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string NhsNumber { get; set; }
        public string Status { get; set; }
        public string BatchReference { get; set; }
        public string UniqueReference { get; set; }
        public DateTimeOffset CacheTime { get; set; }
        public DateTimeOffset LastSentToMesh { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
