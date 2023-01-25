// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public interface IDocumentService
    {
        ValueTask AddDocumentAsync(Document documentName);
    }
}
