using BlazorApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;

        public ThingspeakEndPoint(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _client = httpClientFactory.CreateClient("thingspeak");
            _config = config;
        }

        [FunctionName("GetMostRecent")]
        public async Task<IActionResult> MostRecentData(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest request)
        {
            string requestPrimary = $"/channels/{_config.GetValue<string>("PrimaryChannel")}/feeds/last.json?api_key={_config.GetValue<string>("PrimaryReadKey")}";
            string requestSecondary = $"/channels/{_config.GetValue<string>("SecondaryChannel")}/feeds/last.json?api_key={_config.GetValue<string>("SecondaryReadKey")}";

            Task<ThingspeakFeedModel> responsePrimary = _client.GetFromJsonAsync<ThingspeakFeedModel>(requestPrimary);
            Task<ThingspeakFeedModel> responseSecondary = _client.GetFromJsonAsync<ThingspeakFeedModel>(requestSecondary);

            WeatherRecord dataToSend = new();
            ThingspeakFeedModel dataPrimary, dataSecondary;

            try { dataPrimary = await responsePrimary; }
            catch (Exception ex) { dataPrimary = null; }

            try { dataSecondary = await responseSecondary; }
            catch (Exception ex) { dataSecondary = null; }

            if (dataPrimary != null)
            {
                dataToSend = fillWeatherRecordMain(dataPrimary);
            }
            if (dataSecondary != null)
            {
                if (dataSecondary.created_at == dataToSend.RecordTime)
                {
                    if (dataSecondary.field1 != null) dataToSend.RainfallTotal = float.Parse(dataSecondary.field1);
                    if (dataSecondary.field2 != null) dataToSend.RainfallRate = float.Parse(dataSecondary.field2);
                }
                else if (dataSecondary.created_at > dataToSend.RecordTime)
                {
                    dataToSend = fillWeatherRecordRain(dataSecondary);
                }
            }

            if (dataToSend.RecordTime != default(DateTime))
            {
                return new OkObjectResult(dataToSend);
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [FunctionName("GetDailyData")]
        public async Task<IActionResult> DailyData(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest request)
        {
            string requestPrimary = $"/channels/{_config.GetValue<string>("PrimaryChannel")}/feeds.json?api_key={_config.GetValue<string>("PrimaryReadKey")}&start={DateTime.Now.Date:yyyy-MM-dd'%20'HH:mm:ss}";
            string requestSecondary = $"/channels/{_config.GetValue<string>("SecondaryChannel")}/feeds.json?api_key={_config.GetValue<string>("SecondaryReadKey")}&start={DateTime.Now.Date:yyyy-MM-dd'%20'HH:mm:ss}";

            Task<ThingSpeakRestResponseModel> responsePrimary = _client.GetFromJsonAsync<ThingSpeakRestResponseModel>(requestPrimary);
            Task<ThingSpeakRestResponseModel> responseSecondary = _client.GetFromJsonAsync<ThingSpeakRestResponseModel>(requestSecondary);

            List<WeatherRecord> dataToSend = new List<WeatherRecord>();
            //ThingSpeakRestResponseModel dataPrimary, dataSecondary;

            try
            {
                var dataPrimary = await responsePrimary;
                foreach (ThingspeakFeedModel item in dataPrimary.feeds)
                {
                    dataToSend.Add(fillWeatherRecordMain(item));
                }
            }
            catch (Exception ex) { }

            try
            {
                var dataSecondary = await responseSecondary;
                foreach (var item in dataSecondary.feeds)
                {
                    WeatherRecord recordToEdit = dataToSend.FirstOrDefault(x => x.RecordTime == item.created_at); //There is only one possible record for each timestamp in thingspeak
                    if (recordToEdit == null)
                    {
                        dataToSend.Add(fillWeatherRecordRain(item));
                    }
                    else
                    {
                        if (item.field1 != null) recordToEdit.RainfallTotal = float.Parse(item.field1);
                        if (item.field2 != null) recordToEdit.RainfallRate = float.Parse(item.field2);
                    }
                }
            }
            catch (Exception ex) { }

            if (dataToSend.Count > 0)
            {
                return new OkObjectResult(dataToSend.OrderByDescending(x => x.RecordTime).ToList());
            }
            else
            {
                return new NotFoundResult();
            }
        }
        private WeatherRecord fillWeatherRecordMain(ThingspeakFeedModel data)
        {
            WeatherRecord temp = new() { RecordTime = data.created_at };
            if (data.field1 != null) temp.Temperature = float.Parse(data.field1);
            if (data.field2 != null) temp.Humidity = float.Parse(data.field2);
            if (data.field3 != null) temp.DewPoint = float.Parse(data.field3);
            if (data.field4 != null) temp.StationPressure = float.Parse(data.field4);
            if (data.field5 != null) temp.SeaLevelPressure = float.Parse(data.field5);
            if (data.field6 != null) temp.WindDirection = int.Parse(data.field6);
            if (data.field7 != null) temp.AvgWindSpeed = float.Parse(data.field7);
            if (data.field8 != null) temp.GustWindSpeed = float.Parse(data.field8);
            return temp;
        }

        private WeatherRecord fillWeatherRecordRain(ThingspeakFeedModel data)
        {
            WeatherRecord temp = new() { RecordTime = data.created_at };
            if (data.field1 != null) temp.RainfallTotal = float.Parse(data.field1);
            if (data.field2 != null) temp.RainfallRate = float.Parse(data.field2);
            return temp;
        }
    }
}


