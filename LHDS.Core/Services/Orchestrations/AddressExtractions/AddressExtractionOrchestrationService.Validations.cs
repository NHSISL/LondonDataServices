// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.IO.Compression;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;

namespace LHDS.Core.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationService
    {
        private void ValidateDataOnProcessData(byte[] data)
        {
            ValidateDataIsNotNull(data);
        }

        private static void ValidateDataIsNotNull(byte[] data)
        {
            if (data is null)
            {
                throw new InvalidArgumentAddressExtractionOrchestrationException(
                    message: "Invalid argument address extraction orchestration exception, " +
                    "please correct the errors and try again.");
            }
        }
    }
}