// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.ColumnDefinitions;
using LHDS.Core.Models.Foundations.DataSetSpecifications;

namespace LHDS.Core.Models.Foundations.DataSetObjects
{
    public class DataSetObject : IKey, IAudit
    {
        public Guid Id { get; set; }
        public Guid DataSetSpecificationId { get; set; }
        public string SupplierObjectName { get; set; }
        public string OurObjectName { get; set; }
        public string ObjectDescription { get; set; }
        public string InterchangeProtocol { get; set; }
        public string PushOrPull { get; set; }
        public string DeletionHandling { get; set; }
        public bool IsSubmissionHeaderObject { get; set; }
        public bool IsTransactionLog { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public DataSetSpecification DataSetSpecification { get; set; }
        public List<ObjectColumn> DataSetObjects { get; set; } = new List<ObjectColumn>();

    }
}
