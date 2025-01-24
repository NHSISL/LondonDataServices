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

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyPolls
{
    public partial class TerminologyPollsControllerTests
    {
        [Fact]
        public void GetAllShouldNotHaveRoleAttribute()
        {
            // given 
            var controllerType = typeof(TerminologyPollsController);
            var methodInfo = controllerType.GetMethod("GetAllTerminologyPollsAsync");

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
        public void GetShouldHaveInvisibleApiAttribute()
        {
            // Given
            var controllerType = typeof(TerminologyPollsController);
            var methodInfo = controllerType.GetMethod("GetTerminologyPollByIdAsync");
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
