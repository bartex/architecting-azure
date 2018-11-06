using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web;

namespace Acme.Function
{
    public static class T6HttpGetMsg
    {
        [FunctionName("T6HttpGetMsg")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string dateParam = req.Query["date"];

            if (String.IsNullOrEmpty(dateParam)) {
                new BadRequestObjectResult("Please pass a count on the query string");
            }

            var jsonResult = new StringBuilder();
            var str = Environment.GetEnvironmentVariable("DatabaseConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var command = $"select * from T6Messages where UpdatedTimeUTC between '{dateParam}' and '{dateParam} 23:59:59' for json path";
                SqlCommand cmd = new SqlCommand(command, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    jsonResult.Append("[]");
                }
                else
                {
                    while (reader.Read())
                    {
                        jsonResult.Append(reader.GetValue(0).ToString());
                    }
                }
            }

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent(jsonResult.ToString(), Encoding.UTF8, "application/json");
            return response;
        }
    }
}
