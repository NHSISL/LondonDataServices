// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.FileNameValidations;
using LHDS.Core.Services.Orchestrations.Ingress;
using LHDS.Core.Services.Orchestrations.TppLandings;

namespace LHDS.Core.Services.Coordinations.TppLandings
{
    public partial class TppLandingCoordinationService : ITppLandingCoordinationService
    {
        private readonly ITppLandingOrchestrationService tppOrchestrationService;
        private readonly IIngressOrchestrationService ingressOrchestrationService;
        private readonly IFileNameValidationService fileNameValidationService;

        private readonly ILoggingBroker loggingBroker;

        public TppLandingCoordinationService(
            ITppLandingOrchestrationService tppOrchestrationService,
            IIngressOrchestrationService ingressOrchestrationService,
            IFileNameValidationService fileNameValidationService,
            ILoggingBroker loggingBroker)
        {
            this.tppOrchestrationService = tppOrchestrationService;
            this.ingressOrchestrationService = ingressOrchestrationService;
            this.fileNameValidationService = fileNameValidationService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Guid> ProcessAsync(string fileName, Guid supplierId) =>
            TryCatch(async () =>
            {
                ValidateArgumentsOnProcess(fileName, supplierId);

                Guid ingestionTrackingId = await this.tppOrchestrationService.ProcessAsync(
                    fileName: fileName,
                    supplierId: supplierId);

                return ingestionTrackingId;
            });

        public ValueTask ReProcessAsync(Guid supplierId) =>
            TryCatch(async () =>
            {
                ValidateArgumentsOnReProcess(supplierId);
                await this.tppOrchestrationService.ReProcessAsync(supplierId);
            });

        public async ValueTask<bool> ShouldValidateFileNameAsync(
            string fileName,
            string includePattern,
            string excludePattern)
            {
                ValidateFileNameOnProcess(fileName);

                bool isFileNameValid = await this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

                return isFileNameValid;
            }
    }
}
