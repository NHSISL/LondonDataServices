using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns.Exceptions
{
    public class FailedObjectColumnStorageException : Xeption
    {
        public FailedObjectColumnStorageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}