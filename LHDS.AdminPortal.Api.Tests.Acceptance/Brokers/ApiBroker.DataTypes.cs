// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataType;
using Microsoft.AspNetCore.Mvc;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string DataTypesRelativeUrl = "api/dataTypes";

        public async ValueTask<ActionResult<DataType>> PostDataTypeAsync(DataType dataType) =>
            await this.apiFactoryClient.PostContentAsync(DataTypesRelativeUrl, dataType);

        public async ValueTask<ActionResult<DataType>> DeleteDataTypeByIdAsync(Guid dataTypeId) =>
            await this.apiFactoryClient.DeleteContentAsync<ActionResult<DataType>>(
                $"{DataTypesRelativeUrl}/{dataTypeId}");
    }
}
