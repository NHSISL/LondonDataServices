// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns.Exceptions
{
    public class InvalidObjectColumnException : Xeption
    {
        public InvalidObjectColumnException(string message)
            : base(message)
        { }
    }
}