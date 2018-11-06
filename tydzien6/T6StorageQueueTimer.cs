using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Acme.Function
{
    public static class T6StorageQueueTimer
    {
        private static int counter = 0;

        [FunctionName("T6StorageQueueTimer")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer,
                                ILogger log,
                                [Queue("t6queue-items", Connection = "AzureWebJobsStorage")] out string queueMessage)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            queueMessage = $"Secret data message: {counter++}";
        }
    }
}
