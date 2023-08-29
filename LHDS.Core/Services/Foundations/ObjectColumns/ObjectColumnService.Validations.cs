using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;

namespace LHDS.Core.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnService
    {
        private void ValidateObjectColumnOnAdd(ObjectColumn objectColumn)
        {
            ValidateObjectColumnIsNotNull(objectColumn);
        }

        private static void ValidateObjectColumnIsNotNull(ObjectColumn objectColumn)
        {
            if (objectColumn is null)
            {
                throw new NullObjectColumnException(message: "ObjectColumn is null.");
            }
        }
    }
}