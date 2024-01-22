// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.OdataResponses;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        public async ValueTask<List<T>> GetAllItemsAsync<T>(string relativeOdataUrl)
        {
            OdataResponse<T> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponse<T>>($"{relativeOdataUrl}");

            return response.Items;
        }
    }
}
