// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Processings.ObjectColumns.Exceptions
{
    public class InvalidArgumentObjectColumnProcessingException : Xeption
    {
        public InvalidArgumentObjectColumnProcessingException(string message)
            : base(message)
        { }
    }
}