// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.AdminPortal.Api.Models.Configurations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.DependencyInjection;

namespace LHDS.AdminPortal.Api.Models.Attributes
{
    // Applies the OData server-driven page size from configuration so it can be
    // tuned per environment without hard-coding PageSize on every controller.
    // A caller may request a smaller page via the "pageSize" querystring value
    // (capped at the configured size, so a client can never enlarge the page);
    // acceptance tests use this to exercise paging and next-link behaviour.
    public class EnableConfiguredQueryAttribute : EnableQueryAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            OdataConfigurations odataConfigurations =
                actionExecutedContext.HttpContext.RequestServices
                    .GetRequiredService<OdataConfigurations>();

            int pageSize = odataConfigurations.PageSize;

            string requestedPageSize =
                actionExecutedContext.HttpContext.Request.Query["pageSize"].ToString();

            if (int.TryParse(requestedPageSize, out int requestedPageSizeValue) && requestedPageSizeValue > 0)
            {
                pageSize = Math.Min(pageSize, requestedPageSizeValue);
            }

            this.PageSize = pageSize;

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
