// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.DataSets;
using Newtonsoft.Json;
using LHDS.Core.Models.Foundations.SpecificationObjects;

namespace LHDS.Core.Models.Foundations.DataSetSpecifications
{
    public class DataSetSpecification : IKey, IAudit
    {
        public Guid Id { get; set; }
        public Guid DataSetId { get; set; }
        public string SupplierSpecificationVersion { get; set; }
        public string OurSpecificationVersion { get; set; }
        public string Notes { get; set; }
        public bool IsMultiAuthorPerBatch { get; set; }
        public string EntityChangeSynchronisation { get; set; }
        public DateTimeOffset? DateReleased { get; set; }
        public DateTimeOffset? DateImplemented { get; set; }
        public DateTimeOffset? DateSuperseded { get; set; }
        public Guid? SupersededById { get; set; }
        public Guid? PresededById { get; set; }
        public bool IsPublished { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset ActiveTo { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        [JsonIgnore]
        public DataSet DataSet { get; set; }
        [JsonIgnore]
        public List<DataSetObject> DataSetObjects { get; set; } = new List<DataSetObject>();
        public List<SpecificationObject> SpecificationObjects { get; set; } = new List<SpecificationObject>();
        public List<DataSetSpecification> SupersededBy { get; set; } = new List<DataSetSpecification>();
        public List<DataSetSpecification> PresededBy { get; set; } = new List<DataSetSpecification>();
    }
}
