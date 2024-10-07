using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects.Exceptions
{
    public class NotFoundSpecificationObjectException : Xeption
    {
        public NotFoundSpecificationObjectException(Guid specificationObjectId)
            : base(message: $"Couldn't find specificationObject with specificationObjectId: {specificationObjectId}.")
        { }
    }
}