// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace LHDS.Functions.OptOut
{
    public class RetrieveOptOutStatusFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IOptOutClient optOutClient;
        private readonly ILogger logger;

        public RetrieveOptOutStatusFunction(
            ILoggingBroker loggingBroker,
            IOptOutClient optOutClient,
            ILoggerFactory loggerFactory)
        {
            this.loggingBroker = loggingBroker;
            this.optOutClient = optOutClient;
            this.logger = loggerFactory.CreateLogger<RetrieveOptOutStatusFunction>();
        }

        [Function("RetrieveOptOutStatusFunction")]
        public void Run(
            [BlobTrigger("optout/in/{name}", Connection = "BlobStorage")] string myBlob, string name)
        {
            this.loggingBroker
                .LogInformation(
                    $"C# Blob trigger function Processing blob\n " +
                    $"Name: optout/in/{{name}} \n Data: {myBlob}");

            try
            {
                Task.Run(async () =>
                {
                    byte[] optOutFile = Encoding.ASCII.GetBytes(myBlob);
                    await optOutClient.RetrieveOptOutStatusAsync(optOutFile, fileName: name);
                }).Wait();
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                this.logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
