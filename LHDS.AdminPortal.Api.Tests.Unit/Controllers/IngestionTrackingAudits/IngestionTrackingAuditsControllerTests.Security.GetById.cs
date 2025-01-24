// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Attrify.Attributes;
using FluentAssertions;
using LHDS.AdminPortal.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditsControllerTests
    {
        [Fact]
        public void GetByIdShouldHaveRoleAttributeWithRoles()
        {
            // given
            var controllerType = typeof(IngestionTrackingAuditsController);
            var methodInfo = controllerType.GetMethod("GetAuditByIdAsync");
            Type attributeType = typeof(AuthorizeAttribute);
            string attributeProperty = "Roles";

            List<string> expectedAttributeValues = new List<string>
            {
                "ISL.LDS.AdminSpa.Administrators",
                "ISL.LDS.AdminSpa.IngestionTrackingAudit"
            };

            // when
            var methodAttribute = methodInfo?
                .GetCustomAttributes(attributeType, inherit: true)
                .FirstOrDefault();

            var controllerAttribute = controllerType
                .GetCustomAttributes(attributeType, inherit: true)
                .FirstOrDefault();

            var attribute = methodAttribute ?? controllerAttribute;

            // then
            attribute.Should().NotBeNull();

            var actualAttributeValue = attributeType
                .GetProperty(attributeProperty)?
                .GetValue(attribute) as string ?? string.Empty;

            var actualAttributeValues = actualAttributeValue?
                .Split(',')
                .Select(role => role.Trim())
                .Where(role => !string.IsNullOrEmpty(role))
                .ToList();

            actualAttributeValues.Should().BeEquivalentTo(expectedAttributeValues);
        }

        [Fact]
        public void GetByIdShouldHaveInvisibleApiAttribute()
        {
            // given
            var controllerType = typeof(IngestionTrackingAuditsController);
            var methodInfo = controllerType.GetMethod("GetAuditByIdAsync");
            Type attributeType = typeof(InvisibleApiAttribute);

            // when
            var methodAttribute = methodInfo?
                .GetCustomAttributes(attributeType, inherit: true)
                .FirstOrDefault();

            var controllerAttribute = controllerType
                .GetCustomAttributes(attributeType, inherit: true)
                .FirstOrDefault();

            var attribute = methodAttribute ?? controllerAttribute;

            // then
            attribute.Should().NotBeNull();
        }
    }
}

