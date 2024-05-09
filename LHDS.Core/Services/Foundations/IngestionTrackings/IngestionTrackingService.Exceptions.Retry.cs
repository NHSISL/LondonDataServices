// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings;

namespace LHDS.Core.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService
    {
        private async ValueTask<IngestionTracking> WithRetry(
            ReturningIngestionTrackingFunction<IngestionTracking> returningIngestionTrackingFunction)
        {
            var attempts = 0;

            while (true)
            {
                try
                {
                    attempts++;
                    return await returningIngestionTrackingFunction();
                }
                catch (Exception ex)
                {
                    if (ex is ForeignKeyConstraintConflictException)
                    {
                        this.loggingBroker
                            .LogInformation(
                                $"Error found. Retry attempt {attempts}/{3}. " +
                                    $"Exception: {ex.Message}");

                        if (attempts == 3)
                        {
                            throw;
                        }

                        Task.Delay(1000).Wait();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}