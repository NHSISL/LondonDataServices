using Xeptions;

namespace LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions
{
    public class SpecificationObjectValidationException : Xeption
    {
        public SpecificationObjectValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}