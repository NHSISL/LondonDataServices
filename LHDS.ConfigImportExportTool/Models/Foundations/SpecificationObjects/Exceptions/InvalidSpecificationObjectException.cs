using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions
{
    public class InvalidSpecificationObjectException : Xeption
    {
        public InvalidSpecificationObjectException(string message)
            : base(message)
        { }
    }
}