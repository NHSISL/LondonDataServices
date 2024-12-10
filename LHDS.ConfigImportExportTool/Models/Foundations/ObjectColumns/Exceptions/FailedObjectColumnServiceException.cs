using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns.Exceptions
{
    public class FailedObjectColumnServiceException : Xeption
    {
        public FailedObjectColumnServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}