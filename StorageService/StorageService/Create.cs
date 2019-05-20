using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace StorageService
{
    public static class Create
    {
        [FunctionName("Create")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            string email = req.Query["email"];
            if (string.IsNullOrEmpty(email))
            {
                return new BadRequestObjectResult("zly");
            }

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


            var storage = new StorageService(config["accountName"], config["key"]);

            await storage.AddAsync(new EmailEntity(email));
            return new OkObjectResult("dodano");
        }
    }
}
