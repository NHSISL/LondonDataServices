using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects.Exceptions
{
    public class SpecificationObjectValidationException : Xeption
    {
        public SpecificationObjectValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}