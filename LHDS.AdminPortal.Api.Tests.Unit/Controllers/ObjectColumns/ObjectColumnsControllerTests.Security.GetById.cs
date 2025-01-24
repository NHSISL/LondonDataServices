// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using LHDS.AdminPortal.Api.Controllers;
using Xunit;
using Attrify.Attributes;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ObjectColumns
{
    public partial class ObjectColumnsControllerTests
    {
        [Fact]
        public void GetByIdShouldNotHaveRoleAttribute()
        {
            // given 
            var controllerType = typeof(ObjectColumnsController);
            var methodInfo = controllerType.GetMethod("GetObjectColumnByIdAsync");
            Type attributeType = typeof(AuthorizeAttribute);
            string attributeProperty = "Roles";

            List<string> expectedAttributeValues = new List<string>()
            {
                "ISL.LDS.AdminSpa.Configurations",
                "ISL.LDS.AdminSpa.Administrators"
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
        public void GetIdShouldHaveInvisibleApiAttribute()
        {
            // Given
            var controllerType = typeof(ObjectColumnsController);
            var methodInfo = controllerType.GetMethod("GetObjectColumnByIdAsync");
            Type attributeType = typeof(InvisibleApiAttribute);

            // When
            var methodAttribute = methodInfo?
                .GetCustomAttributes(attributeType, inherit: true)
                .FirstOrDefault();

            var controllerAttribute = controllerType
                .GetCustomAttributes(attributeType, inherit: true)
                .FirstOrDefault();

            var attribute = methodAttribute ?? controllerAttribute;

            // Then
            attribute.Should().NotBeNull();
        }
    }
}
