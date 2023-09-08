using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class NotFoundSpecificationObjectException : Xeption
    {
        public NotFoundSpecificationObjectException(Guid dataSetObjectId)
            : base(message: $"Couldn't find dataSetObject with dataSetObjectId: {dataSetObjectId}.")
        { }
    }
}