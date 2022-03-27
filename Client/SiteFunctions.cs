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
    }
}
