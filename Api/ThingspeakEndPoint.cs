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
        private readonly HttpClient _client;

        public ThingspeakEndPoint(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("thingspeak");
        }

        [FunctionName("GetDailyData")]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest request)
        {
            Task<ThingSpeakRestResponseModel> responseA = Task.Run(async () =>
            {
                ThingSpeakRestResponseModel response;
                try
                {
                    response = await _client.GetFromJsonAsync<ThingSpeakRestResponseModel>($"/channels/1646398/feeds.json?api_key=17RI7ZVUCL1DFKMR&start={DateTime.Now.Date:yyyy-MM-dd'%20'HH:mm:ss}");
                }
                catch (Exception ex) { response = null; }

                return response;
            });

            Task<ThingSpeakRestResponseModel> responseB = Task.Run(async () =>
            {
                ThingSpeakRestResponseModel response;
                try
                {
                    response = await _client.GetFromJsonAsync<ThingSpeakRestResponseModel>($"/channels/1646399/feeds.json?api_key=4ZFCB9F8S5XD1RM7&start={DateTime.Now.Date:yyyy-MM-dd'%20'HH:mm:ss}");
                }
                catch (Exception ex) { response = null; }

                return response;
            });

            List<WeatherRecord> dataToSend = new List<WeatherRecord>();

            var mainData = await responseA;
            var rainData = await responseB;

            if (mainData != null)
            {
                foreach(ThingspeakFeedModel item in mainData.feeds)
                {
                    var temp = new WeatherRecord() { RecordTime = item.created_at };
                    if (item.field1 != null) temp.Temperature = float.Parse(item.field1);
                    if (item.field2 != null) temp.Humidity = float.Parse(item.field2);
                    if (item.field3 != null) temp.DewPoint = float.Parse(item.field3);
                    if (item.field4 != null) temp.StationPressure = float.Parse(item.field4);
                    if (item.field5 != null) temp.SeaLevelPressure = float.Parse(item.field5);
                    if (item.field6 != null) temp.WindDirection = int.Parse(item.field6);
                    if (item.field7 != null) temp.AvgWindSpeed = float.Parse(item.field7);
                    if (item.field8 != null) temp.GustWindSpeed = float.Parse(item.field8);
                    dataToSend.Add(temp);
                }
            }
            if (rainData != null)
            {
                foreach(var item in rainData.feeds)
                {
                    WeatherRecord recordToEdit = dataToSend.FirstOrDefault(x => x.RecordTime == item.created_at); //There is only one possible record for each timestamp in thingspeak
                    if (recordToEdit == null)
                    {
                        recordToEdit = new WeatherRecord() { RecordTime = item.created_at };
                        recordToEdit.RecordTime = item.created_at;
                    }
                    if (item.field1 != null) recordToEdit.RainfallTotal = float.Parse(item.field1);
                    if (item.field2 != null) recordToEdit.RainfallRate = float.Parse(item.field2);
                }
            }

            if (dataToSend.Count > 0)
            {
                return new OkObjectResult(dataToSend.OrderByDescending(x => x.RecordTime).ToList());
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}

