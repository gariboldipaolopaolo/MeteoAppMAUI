using Newtonsoft.Json;
using System.Diagnostics;

namespace MeteoApp
{
    internal class RestService
    {
        readonly HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
        }

        public async Task<WeatherData> GetWeatherData(string query)
        {
            WeatherData weatherData = null;

            try
            {
                var response = await _client.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    weatherData = JsonConvert.DeserializeObject<WeatherData>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return weatherData;
        }

        public async Task<Models.Location> GetCity(string query)
        {
            Models.Location location = null;

            try
            {
                _client.DefaultRequestHeaders.Add("X-Api-Key", Constants.LocationAPIKey);
                var response = await _client.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    content = content.Replace("[", "").Replace("]", "");
                    location = JsonConvert.DeserializeObject<Models.Location>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return location;
        }
    }
}