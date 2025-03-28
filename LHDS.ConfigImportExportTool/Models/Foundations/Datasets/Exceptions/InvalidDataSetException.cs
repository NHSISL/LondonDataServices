// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions
{
    public class InvalidDataSetException : Xeption
    {
        public InvalidDataSetException(string message)
            : base(message)
        { }
    }
}