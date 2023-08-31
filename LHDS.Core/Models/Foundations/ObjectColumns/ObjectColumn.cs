// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.DataSetObjects;

namespace LHDS.Core.Models.Foundations.ObjectColumns
{
    public class ObjectColumn : IKey, IAudit
    {
        public Guid Id { get; set; }
        public Guid DataSetObjectId { get; set; }
        public string SupplierColumnName { get; set; }
        public string OurColumnName { get; set; }
        public string ColumnDescription { get; set; }
        public int OrdinalPosition { get; set; }
        public string PopulatedBy { get; set; }
        public string SqlDataType { get; set; }
        public int? Length { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public string FhirDataType { get; set; }
        public string SupplierDateFormat { get; set; }
        public bool IsWatermark { get; set; }
        public bool IsSequencing { get; set; }
        public bool IsEntityBusinessKey { get; set; }
        public bool IsRecordBusinessKey { get; set; }
        public bool IsMutable { get; set; }
        public bool IsSenderCode { get; set; }
        public bool IsAuthorCode { get; set; }
        public bool IsDeleteFlag { get; set; }
        public bool IsPersonConfidentialData { get; set; }
        public string TypeOfPersonConfidentialData { get; set; }
        public string MaskingMethod { get; set; }
        public bool IsSensitiveRecordMarker { get; set; }
        public string CodeSystem { get; set; }
        public string PartitionColumnLevel { get; set; }
        public Guid DataTypeId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public DataSetObject DataSetObject { get; set; }
    }
}
