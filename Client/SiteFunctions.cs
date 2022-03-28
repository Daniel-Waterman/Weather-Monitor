namespace BlazorApp.Client
{
    public static class SiteFunctions
    {
        public static float celsiusToFahrenheit(float temperatureC)
        {
            throw new NotImplementedException();
        }

        public static float celsiusToKelvin(float temperatureC)
        {
            throw new NotImplementedException();
        }

        public static float mmToInch(float rainMM)
        {
            throw new NotImplementedException();
        }

        public static float kmhToMph(float windKph)
        {
            throw new NotImplementedException();
        }

        public static float kmhToKnot(float windKph)
        {
            throw new NotImplementedException();
        }

        public static float hPaToInHg(float pressureHpa)
        {
            throw new NotImplementedException();
        }

        public static string GetDisplayFloat(float value, int decimalPlaces)
        {
            if (value == -99 || value == 2000) // Default values
            {
                return "Err";
            }
            else
            {
                return Math.Round(value, decimalPlaces).ToString();
            }
        }

        public static string GetWindDirectionString(int value)
        {
            if (value < 0) return "Err";
            else if (value < 11.25) return "N";
            else if (value < 33.75) return "NNE";
            else if (value < 56.25) return "NE";
            else if (value < 78.75) return "ENE";
            else if (value < 101.25) return "E";
            else if (value < 123.75) return "ESE";
            else if (value < 146.25) return "SE";
            else if (value < 168.75) return "SSE";
            else if (value < 191.25) return "S";
            else if (value < 213.75) return "SSW";
            else if (value < 236.25) return "SW";
            else if (value < 258.75) return "WSW";
            else if (value < 281.25) return "W";
            else if (value < 303.75) return "WNW";
            else if (value < 326.25) return "NW";
            else if (value < 348.75) return "NNW";
            else if (value <= 360) return "N";
            else return "Err";
        }
    }
}
