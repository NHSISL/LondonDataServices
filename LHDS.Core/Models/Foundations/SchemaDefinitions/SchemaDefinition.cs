// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.ColumnDefinitions;
using LHDS.Core.Models.Foundations.DataSets;

namespace LHDS.Core.Models.Foundations.SchemaDefinitions
{
    public class SchemaDefinition : IKey, IAudit
    {
        public Guid Id { get; set; }
        public Guid DataSetId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public DataSet DataSet { get; set; }
        public List<ColumnDefinition> ColumnDefinitions { get; set; } = new List<ColumnDefinition>();

    }
}
