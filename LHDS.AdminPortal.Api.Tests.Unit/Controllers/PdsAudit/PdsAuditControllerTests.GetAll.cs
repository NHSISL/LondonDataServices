// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.PdsAudit
{
    public partial class PdsAuditControllerTests
    {
        [Fact]
        public void GetAllShouldHaveRoleAttributeWithRoles()
        {
            // given
            var controllerType = typeof(PdsAuditsController);
            var methodInfo = controllerType.GetMethod("Get");
            Type attributeType = typeof(AuthorizeAttribute);
            string attributeProperty = "Roles";

            List<string> expectedAttributeValues = new List<string>
            {
                "ISL.LDS.AdminSpa.Administrators",
                "ISL.LDS.AdminSpa.ReadOnly",
                "ISL.LDS.AdminSpa.Pds"
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
    }
}
