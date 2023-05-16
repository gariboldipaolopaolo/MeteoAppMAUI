using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteoApp.Models
{
    public class Location
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("population")]
        public int Population { get; set; }
        [JsonProperty("is_capital")]
        public bool IsCapital{ get; set; }
    }
}
