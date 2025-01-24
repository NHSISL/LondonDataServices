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

            // when
            var methodAttribute = methodInfo?
                .GetCustomAttributes(attributeType, inherit: true)
                .FirstOrDefault();

            var controllerAttribute = controllerType
                .GetCustomAttributes(attributeType, inherit: true)
                .FirstOrDefault();

            var attribute = methodAttribute ?? controllerAttribute;

            // then
            attribute.Should().BeNull();
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
