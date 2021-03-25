using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using System.Collections.Generic;
using PlayFab.Samples;

namespace Tempera
{
    public static class SetDataRegister
    {
        [FunctionName("SetDataRegister")]
        public static async Task<dynamic> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            FunctionExecutionContext<dynamic> context = JsonConvert.DeserializeObject<FunctionExecutionContext<dynamic>>(await req.ReadAsStringAsync());

            var apiSettings = new PlayFabApiSettings
            {
                TitleId = context.TitleAuthenticationContext.Id,
                DeveloperSecretKey = Environment.GetEnvironmentVariable("PLAYFAB_DEV_SECRET_KEY", EnvironmentVariableTarget.Process),
            };
            var serverApi = new PlayFabServerInstanceAPI(apiSettings);
            var valueToUp = 1;
            var request = new PlayFab.ServerModels.UpdatePlayerStatisticsRequest();
            var stat = new PlayFab.ServerModels.StatisticUpdate { StatisticName = "Level", Value = valueToUp };
            request.Statistics = new List<PlayFab.ServerModels.StatisticUpdate> { stat };
            request.PlayFabId = context.CallerEntityProfile.Lineage.MasterPlayerAccountId;
            var result = await serverApi.UpdatePlayerStatisticsAsync(request);
            return valueToUp;
        }
    }
}
