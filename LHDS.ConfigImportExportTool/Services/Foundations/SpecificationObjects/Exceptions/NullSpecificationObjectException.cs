using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects.Exceptions
{
    public class NullSpecificationObjectException : Xeption
    {
        public NullSpecificationObjectException(string message)
            : base(message)
        { }
    }
}