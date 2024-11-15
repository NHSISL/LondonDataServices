using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions
{
    public class NotFoundSpecificationObjectException : Xeption
    {
        public NotFoundSpecificationObjectException(string message)
            : base(message)
        { }
    }
}