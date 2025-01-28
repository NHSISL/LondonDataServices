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
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;
using Moq;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SubscriberCredentials
{
    public partial class SubscriberCredentialsControllerTests
    {
        private readonly Mock<ISubscriberCredentialOrchestration> subscriberCredentialOrchestrationMock;
        private readonly SubscriberCredentialsController subscriberCredentialController;

        public SubscriberCredentialsControllerTests()
        {
            this.subscriberCredentialOrchestrationMock = new Mock<ISubscriberCredentialOrchestration>();
            
            this.subscriberCredentialController = new SubscriberCredentialsController(
                this.subscriberCredentialOrchestrationMock.Object);
        }
    }
}
