using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions
{
    public class NotFoundDataSetObjectException : Xeption
    {
        public NotFoundDataSetObjectException(Guid dataSetObjectId)
            : base(message: $"Couldn't find dataSetObject with dataSetObjectId: {dataSetObjectId}.")
        { }
    }
}