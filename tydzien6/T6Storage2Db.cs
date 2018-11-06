using System;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Acme.Function
{
    public static class T6Storage2Db
    {
        [FunctionName("T6Storage2Db")]
        public static async Task Run([QueueTrigger("t6queue-items", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var str = Environment.GetEnvironmentVariable("DatabaseConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var command = $"Insert into T6Messages (TextMessage, UpdatedTimeUTC) values ('{myQueueItem}', GETUTCDATE())";

                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    var rows = await cmd.ExecuteNonQueryAsync();
                    log.LogInformation($"{rows} rows were updated");
                }
            }
        }
    }
}
