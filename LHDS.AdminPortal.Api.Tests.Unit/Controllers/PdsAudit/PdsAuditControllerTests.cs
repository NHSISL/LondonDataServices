// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Controllers;
using LHDS.Core.Services.Foundations.PdsAudits;
using LHDS.Core.Services.Foundations.Suppliers;
using Moq;
using RESTFulSense.Controllers;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.PdsAudit
{
    public partial class PdsAuditControllerTests
    {
        private readonly Mock<IPdsAuditService> pdsAuditServiceMock;
        private readonly PdsAuditsController pdsAuditsController;

        public PdsAuditControllerTests()
        {
            this.pdsAuditServiceMock = new Mock<IPdsAuditService>();
            this.pdsAuditsController = new PdsAuditsController(this.pdsAuditServiceMock.Object);
        }
    }
}
