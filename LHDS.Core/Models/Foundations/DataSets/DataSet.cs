// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.SchemaDefinitions;

namespace LHDS.Core.Models.Foundations.DataSets
{
    public class DataSet : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public List<DatasetObject> SchemaDefinitions { get; set; } = new List<DatasetObject>();
    }
}
