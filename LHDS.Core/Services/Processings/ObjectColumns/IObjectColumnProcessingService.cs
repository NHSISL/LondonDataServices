// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ObjectColumns;

namespace LHDS.Core.Services.Processings.ObjectColumns
{
    public interface IObjectColumnProcessingService
    {
        ValueTask<ObjectColumn> AddObjectColumnAsync(ObjectColumn objectColumn);
    }
}
