using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DataSets.Exceptions
{
    public class NotFoundDataSetException : Xeption
    {
        public NotFoundDataSetException(Guid dataSetId)
            : base(message: $"Couldn't find dataSet with dataSetId: {dataSetId}.")
        { }
    }
}