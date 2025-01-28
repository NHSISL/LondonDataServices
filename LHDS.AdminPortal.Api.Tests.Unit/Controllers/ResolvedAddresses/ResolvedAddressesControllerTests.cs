// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.Suppliers;
using Moq;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.ResolvedAddresses
{
    public partial class ResolvedAddressesControllerTests
    {
        private readonly Mock<IResolvedAddressService> resolvedAddressServiceMock;
        private readonly ResolvedAddressesController resolvedAddressesController;

        public ResolvedAddressesControllerTests()
        {
            this.resolvedAddressServiceMock = new Mock<IResolvedAddressService>();
            this.resolvedAddressesController = new ResolvedAddressesController(this.resolvedAddressServiceMock.Object);
        }
    }
}