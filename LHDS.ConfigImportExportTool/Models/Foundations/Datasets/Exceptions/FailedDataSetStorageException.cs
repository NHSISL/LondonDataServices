using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions
{
    public class FailedDataSetStorageException : Xeption
    {
        public FailedDataSetStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}