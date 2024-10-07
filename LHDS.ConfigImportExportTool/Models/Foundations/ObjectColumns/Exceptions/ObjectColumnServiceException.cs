using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns.Exceptions
{
    public class ObjectColumnServiceException : Xeption
    {
        public ObjectColumnServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}