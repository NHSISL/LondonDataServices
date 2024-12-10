using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions
{
    public class AlreadyExistsSpecificationObjectException : Xeption
    {
        public AlreadyExistsSpecificationObjectException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}