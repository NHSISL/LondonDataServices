// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.AdminPortal.Api.Models.Configurations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.AdminPortal.Api.Models.Attributes
{
    // Applies the OData server-driven page size from configuration so it can be
    // tuned per environment (and overridden to a small value in acceptance tests)
    // without hard-coding PageSize on every controller.
    public class EnableConfiguredQueryAttribute : EnableQueryAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            OdataConfigurations odataConfigurations =
                actionExecutedContext.HttpContext.RequestServices
                    .GetRequiredService<OdataConfigurations>();

            this.PageSize = odataConfigurations.PageSize;

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
