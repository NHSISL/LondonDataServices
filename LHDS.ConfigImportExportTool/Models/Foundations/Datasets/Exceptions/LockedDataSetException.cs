using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions
{
    public class LockedDataSetException : Xeption
    {
        public LockedDataSetException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}