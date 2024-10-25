using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects.Exceptions
{
    public class InvalidSpecificationObjectReferenceException : Xeption
    {
        public InvalidSpecificationObjectReferenceException(string message, Exception? innerException)
            : base(message, innerException) { }
    }
}