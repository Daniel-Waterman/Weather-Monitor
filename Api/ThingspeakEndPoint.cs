using BlazorApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Api
{
    public class ThingspeakEndPoint
    {
        private readonly HttpRequestMessage requestA = new HttpRequestMessage(HttpMethod.Get, "/channels/1646398/feeds.json?api_key=17RI7ZVUCL1DFKMR&results=1");
        private readonly HttpRequestMessage requestB = new HttpRequestMessage(HttpMethod.Get, "/channels/1646399/feeds.json?api_key=4ZFCB9F8S5XD1RM7&results=1");
        private readonly HttpClient _client;

        public ThingspeakEndPoint(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("thingspeak");
        }

        [FunctionName("GetLatestData")]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest request)
        {
            ThingSpeakRestResponseModel mainData = null;
            ThingSpeakRestResponseModel rainData = null;

            Task<HttpResponseMessage> responseA = _client.SendAsync(requestA);
            Task<HttpResponseMessage> responseB = _client.SendAsync(requestB);
            var responseMain = await responseA;
            var responseRain = await responseB;

            if (responseMain.IsSuccessStatusCode)
            {
                mainData = await responseMain.Content.ReadFromJsonAsync<ThingSpeakRestResponseModel>();
            }

            if (responseRain.IsSuccessStatusCode)
            {
                rainData = await responseRain.Content.ReadFromJsonAsync<ThingSpeakRestResponseModel>();
            }

            SingleWeatherData dataToSend = new();

            if (mainData != null)
            {
                dataToSend.MainRecordTime = mainData.feeds.First().created_at;
                dataToSend.Temperature = float.Parse(mainData.feeds.First().field1);
                dataToSend.Humidity = float.Parse(mainData.feeds.First().field2);
                dataToSend.DewPoint = float.Parse(mainData.feeds.First().field3);
                dataToSend.StationPressure = float.Parse(mainData.feeds.First().field4);
                dataToSend.SeaLevelPressure = float.Parse(mainData.feeds.First().field5);
                dataToSend.AvgWindSpeed = float.Parse(mainData.feeds.First().field7);
                dataToSend.GustWindSpeed = float.Parse(mainData.feeds.First().field8);
                dataToSend.WindDirection = int.Parse(mainData.feeds.First().field6);
            }
            if (rainData != null)
            {
                dataToSend.RainRecordTime = rainData.feeds.First().created_at;
                dataToSend.RainfallTotal = float.Parse(rainData.feeds.First().field1);
                dataToSend.RainfallRate = float.Parse(rainData.feeds.First().field2);
            }

            if (dataToSend.MainRecordTime != null || dataToSend.RainRecordTime != null)
            {
                return new OkObjectResult(dataToSend);
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}

