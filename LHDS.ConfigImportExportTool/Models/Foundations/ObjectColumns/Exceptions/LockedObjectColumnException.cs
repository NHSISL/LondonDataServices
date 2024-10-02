using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns.Exceptions
{
    public class LockedObjectColumnException : Xeption
    {
        public LockedObjectColumnException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}