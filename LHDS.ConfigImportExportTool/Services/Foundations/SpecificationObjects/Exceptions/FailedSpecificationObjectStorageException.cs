using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects.Exceptions
{
    public class FailedSpecificationObjectStorageException : Xeption
    {
        public FailedSpecificationObjectStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}