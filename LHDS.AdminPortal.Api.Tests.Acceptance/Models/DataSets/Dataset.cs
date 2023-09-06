// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets
{
    public class DataSet
    {
        public Guid Id { get; set; }
        public string DataSetName { get; set; }
        public string DataSetAliasses { get; set; }
        public string DataSetSupplier { get; set; }
        public string DataSetAuthor { get; set; }
        public string DataSourceType { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset? ActiveFrom { get; set; }
        public DateTimeOffset? ActiveTo { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

    }
}
