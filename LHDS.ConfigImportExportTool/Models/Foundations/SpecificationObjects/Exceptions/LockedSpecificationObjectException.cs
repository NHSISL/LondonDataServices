using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions
{
    public class LockedSpecificationObjectException : Xeption
    {
        public LockedSpecificationObjectException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}