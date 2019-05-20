using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SenderService
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task Run([QueueTrigger("queue")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var list = await Mail.GetMailListAsync();

            foreach (var p in list)
            {
                try
                {
                    await new Mail().CreateMailAsync(p, "test@test.pl", "zdane", "Zdane", "", "Tak jest !" + myQueueItem);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}
