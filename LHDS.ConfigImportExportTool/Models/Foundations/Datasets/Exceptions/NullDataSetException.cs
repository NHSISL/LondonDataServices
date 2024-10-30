using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions
{
    public class NullDataSetException : Xeption
    {
        public NullDataSetException(string message)
            : base(message)
        { }
    }
}