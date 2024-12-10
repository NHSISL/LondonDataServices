using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns.Exceptions
{
    public class InvalidObjectColumnReferenceException : Xeption
    {
        public InvalidObjectColumnReferenceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}