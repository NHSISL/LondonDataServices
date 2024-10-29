using System;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions
{
    public class NotFoundDataSetException : Xeption
    {
        public NotFoundDataSetException(Guid dataSetId)
            : base(message: $"Couldn't find dataSet with dataSetId: {dataSetId}.")
        { }
    }
}