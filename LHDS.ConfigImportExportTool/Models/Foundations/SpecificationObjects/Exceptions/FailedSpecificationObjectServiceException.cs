using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions
{
    public class FailedSpecificationObjectServiceException : Xeption
    {
        public FailedSpecificationObjectServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}