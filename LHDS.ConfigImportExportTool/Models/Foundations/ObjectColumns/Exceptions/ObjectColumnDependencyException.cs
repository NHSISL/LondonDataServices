// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns.Exceptions
{
    public class ObjectColumnDependencyException : Xeption
    {
        public ObjectColumnDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}