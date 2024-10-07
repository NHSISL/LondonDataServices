using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects.Exceptions
{
    public class InvalidSpecificationObjectException : Xeption
    {
        public InvalidSpecificationObjectException(string message)
            : base(message)
        { }
    }
}