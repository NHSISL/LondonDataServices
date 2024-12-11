// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LHDS.Core.Models.Foundations.ObjectColumns
{
    public class ObjectColumn : IKey, IAudit
    {
        public Guid Id { get; set; }
        public Guid SpecificationObjectId { get; set; }
        public string SupplierColumnName { get; set; } = string.Empty;
        public string OurColumnName { get; set; } = string.Empty;
        public string ColumnDescription { get; set; } = string.Empty;
        public int OrdinalPosition { get; set; }
        public string PopulatedBy { get; set; } = string.Empty;
        public string FhirDataType { get; set; } = string.Empty;
        public string SqlDataType { get; set; } = string.Empty;
        public int? Length { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public string SupplierDateFormat { get; set; } = string.Empty;
        public bool IsWatermark { get; set; }
        public bool IsSequencing { get; set; }
        public bool IsBusinessKey { get; set; }
        public bool IsUniqueRecordKey { get; set; }
        public bool IsVersionHashElement { get; set; }
        public bool IsSenderCode { get; set; }
        public bool IsAuthorCode { get; set; }
        public bool IsRelatedOrganisationId { get; set; }
        public bool IsDeleteFlag { get; set; }
        public bool IsSensitiveRecordMarker { get; set; }
        public bool IsPersonConfidentialData { get; set; }
        public string PersonConfidentialDataType { get; set; } = string.Empty;
        public string MaskingMethod { get; set; } = string.Empty;
        public string CodeSystem { get; set; } = string.Empty;
        public string PartitionColumnLevel { get; set; } = string.Empty;
        public Guid DataTypeId { get; set; }
        public bool IsForeignKey { get; set; } = false;
        public string ForeignKeyTableName { get; set; } = string.Empty;
        public string ForeignKeyColumnName { get; set; } = string.Empty;
        public bool IsCaseSensitive { get; set; } = false;
        public string DeleteCondition { get; set; } = string.Empty;
        public bool IsPostcode { get; set; } = false;
        public bool IsNumeric { get; set; } = false;
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        [BindNever]
        public SpecificationObject? SpecificationObject { get; set; } = null!;
    }
}
