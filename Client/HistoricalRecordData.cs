using BlazorApp.Shared;

namespace BlazorApp.Client
{
    public class HistoricalRecordData
    {
        public ExtremeRecord TemperatureMaxMin = new ExtremeRecord();
        public ExtremeRecord HumidityMaxMin = new ExtremeRecord();
        public ExtremeRecord DewPointMaxMin = new ExtremeRecord();
        public ExtremeRecord PressureMaxMin = new ExtremeRecord();

        public float maxAvgWind = 0;
        public float maxGustWind = 0;
        public float maxRainRate = 0;
        public DateTime maxAvgWindTime;
        public DateTime maxGustWindTime;
        public DateTime maxRainRateTime;

        public void NewData(List<WeatherRecord> data)
        {
            foreach (WeatherRecord record in data)
            {
                if (record.Temperature != -99)
                {
                    if (record.Temperature > TemperatureMaxMin.MaxValue)
                    {
                        TemperatureMaxMin.MaxValue = record.Temperature;
                        TemperatureMaxMin.MaxValueTime = record.RecordTime;
                    }
                    if (record.Temperature < TemperatureMaxMin.MinValue)
                    {
                        TemperatureMaxMin.MinValue = record.Temperature;
                        TemperatureMaxMin.MinValueTime = record.RecordTime;
                    }
                }

                if (record.Humidity != -99)
                {
                    if (record.Humidity > HumidityMaxMin.MaxValue)
                    {
                        HumidityMaxMin.MaxValue = record.Humidity;
                        HumidityMaxMin.MaxValueTime = record.RecordTime;
                    }
                    if (record.Humidity < HumidityMaxMin.MinValue)
                    {
                        HumidityMaxMin.MinValue = record.Humidity;
                        HumidityMaxMin.MinValueTime = record.RecordTime;
                    }
                }

                if (record.DewPoint != -99)
                {
                    if (record.DewPoint > DewPointMaxMin.MaxValue)
                    {
                        DewPointMaxMin.MaxValue = record.DewPoint;
                        DewPointMaxMin.MaxValueTime = record.RecordTime;
                    }
                    if (record.DewPoint < DewPointMaxMin.MinValue)
                    {
                        DewPointMaxMin.MinValue = record.DewPoint;
                        DewPointMaxMin.MinValueTime = record.RecordTime;
                    }
                }

                if (record.SeaLevelPressure != -99)
                {
                    if (record.SeaLevelPressure > PressureMaxMin.MaxValue)
                    {
                        PressureMaxMin.MaxValue = record.SeaLevelPressure;
                        PressureMaxMin.MaxValueTime = record.RecordTime;
                    }
                    if (record.SeaLevelPressure < PressureMaxMin.MinValue)
                    {
                        PressureMaxMin.MinValue = record.SeaLevelPressure;
                        PressureMaxMin.MinValueTime = record.RecordTime;
                    }
                }               

                if (record.AvgWindSpeed != -99 && record.AvgWindSpeed > maxAvgWind)
                {
                    maxAvgWind = record.AvgWindSpeed;
                    maxAvgWindTime = record.RecordTime;
                }

                if (record.GustWindSpeed != -99 && record.GustWindSpeed > maxGustWind)
                {
                    maxGustWind = record.GustWindSpeed;
                    maxGustWindTime = record.RecordTime;
                }

                if (record.RainfallRate != -99 && record.RainfallRate > maxRainRate)
                {
                    maxRainRate = record.RainfallRate;
                    maxRainRateTime = record.RecordTime;
                }
            }
        }
    }

    public struct ExtremeRecord
    {
        public float MinValue { get; set; } = 2000;
        public float MaxValue { get; set; } = -99;
        public DateTime MinValueTime { get; set; }
        public DateTime MaxValueTime { get; set; }
    }
}
