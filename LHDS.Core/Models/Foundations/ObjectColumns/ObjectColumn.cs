// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.SchemaDefinitions;

namespace LHDS.Core.Models.Foundations.ColumnDefinitions
{
    public class ObjectColumn : IKey, IAudit
    {
        public Guid Id { get; set; }
        public Guid DatasetObjectId { get; set; }
        public Guid DataTypeId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public DatasetObject DatasetObject { get; set; }
    }
}
