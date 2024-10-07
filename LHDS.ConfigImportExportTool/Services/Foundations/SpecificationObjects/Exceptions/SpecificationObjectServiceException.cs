using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects.Exceptions
{
    public class SpecificationObjectServiceException : Xeption
    {
        public SpecificationObjectServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}