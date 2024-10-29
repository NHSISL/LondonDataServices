using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions
{
    public class InvalidDataSetReferenceException : Xeption
    {
        public InvalidDataSetReferenceException(string message, Exception? innerException)
            : base(message, innerException) { }
    }
}