using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions
{
    public class AlreadyExistsDataSetException : Xeption
    {
        public AlreadyExistsDataSetException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}