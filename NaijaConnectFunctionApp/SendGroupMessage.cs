using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using NaijaConnect.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NaijaConnectFunctionApp
{
    public static class SendGroupMessage
    {
        [FunctionName("SendGroupMessage")]
        public async static Task Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] object message,
            [SignalR(HubName = "live")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var json = message.ToString();
            var msg = JsonConvert.DeserializeObject<Message>(json);

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = msg.TypeInfo,
                    Arguments = new[] { message }
                });
        }
    }
}
