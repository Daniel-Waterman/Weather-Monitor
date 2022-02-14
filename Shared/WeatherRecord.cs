using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared
{
    public class WeatherRecord
    {
        public DateTime RecordTime { get; set; }
        public float Temperature { get; set; } = -99;
        public float Humidity { get; set; } = -99;
        public float DewPoint { get; set; } = -99;
        public float StationPressure { get; set; } = -99;
        public float SeaLevelPressure { get; set; } = -99;
        public float AvgWindSpeed { get; set; } = -99;
        public float GustWindSpeed { get; set; } = -99;
        public int WindDirection { get; set; } = -99;
        public float RainfallTotal { get; set; } = -99;
        public float RainfallRate { get; set; } = -99;
    }
}
