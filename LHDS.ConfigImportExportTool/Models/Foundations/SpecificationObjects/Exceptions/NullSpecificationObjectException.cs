using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions
{
    public class NullSpecificationObjectException : Xeption
    {
        public NullSpecificationObjectException(string message)
            : base(message)
        { }
    }
}