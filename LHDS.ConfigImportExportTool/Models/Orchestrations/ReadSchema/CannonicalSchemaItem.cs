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
        public int ColumnLength { get; set; }
        public int ColumnOrdinal { get; set; }
        public string LinkedTable { get; set; }
        public string LinkedColumn { get; set; }
    }
}
