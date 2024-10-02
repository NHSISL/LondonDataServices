using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns.Exceptions
{
    public class NotFoundObjectColumnException : Xeption
    {
        public NotFoundObjectColumnException(Guid objectColumnId)
            : base(message: $"Couldn't find objectColumn with objectColumnId: {objectColumnId}.")
        { }
    }
}