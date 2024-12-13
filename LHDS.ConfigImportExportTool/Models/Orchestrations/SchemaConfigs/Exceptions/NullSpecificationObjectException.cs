// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions
{
    public class NullSpecificationObjectListException : Xeption
    {
        public NullSpecificationObjectListException(string message)
            : base(message)
        { }
    }
}