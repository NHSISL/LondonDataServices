// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema
{
    public class CannonicalSchemaItem
    {
        public string TableName { get; set; }
        public string TableDescription { get; set; }
        public string ColumnName { get; set; }
        public string ColumnDescription { get; set; }
        public string ColumnDataType { get; set; }
        public int? ColumnLength { get; set; }
        public int ColumnOrdinal { get; set; }
        public string LinkedTable { get; set; }
        public string LinkedColumn { get; set; }
        public string DataSetSpecificationId { get; set; }
        public string OurObjectName { get; set; } = string.Empty;
        public string InterchangeProtocol { get; set; } = string.Empty;
        public bool? IsPushedToUs { get; set; }
        public bool? IsPulledByUs { get; set; }
        public string DeletionHandling { get; set; } = string.Empty;
        public bool? IsSubmissionHeaderObject { get; set; }
        public bool? IsTransactionLog { get; set; }
        public string SpecificationObjectId { get; set; }
        public string OurColumnName { get; set; } = string.Empty;
        public string PopulatedBy { get; set; } = string.Empty;
        public string FhirDataType { get; set; } = string.Empty;
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public string SupplierDateFormat { get; set; } = string.Empty;
        public bool? IsWatermark { get; set; }
        public bool? IsSequencing { get; set; }
        public bool? IsBusinessKey { get; set; }
        public bool? IsUniqueRecordKey { get; set; }
        public bool? IsVersionHashElement { get; set; }
        public bool? IsSenderCode { get; set; }
        public bool? IsAuthorCode { get; set; }
        public bool? IsRelatedOrganisationId { get; set; }
        public bool? IsDeleteFlag { get; set; }
        public bool? IsSensitiveRecordMarker { get; set; }
        public bool? IsPersonConfidentialData { get; set; }
        public string PersonConfidentialDataType { get; set; } = string.Empty;
        public string MaskingMethod { get; set; } = string.Empty;
        public string CodeSystem { get; set; } = string.Empty;
        public string PartitionColumnLevel { get; set; } = string.Empty;
        public string DataTypeId { get; set; }
        public bool? IsForeignKey { get; set; } = false;
    }
}
