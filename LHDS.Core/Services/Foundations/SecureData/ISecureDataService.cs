// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SecureData;

namespace LHDS.Core.Services.Foundations.SecureDatas
{
    public interface ISecureDataService
    {
        ValueTask<SecureData> AddOrModifySecureData(SecureData secureData);
    }
}