using System.Text.Json.Serialization;

namespace FluentSearchEngine.Model
{
    public class GeoCoordinates
    {
        public GeoCoordinates()
        {

        }

        [JsonPropertyName("lat")]
        private string Latitude { get; set; }
        [JsonPropertyName("lng")]
        private string Longitude { get; set; }

        public GeoCoordinates(string latitude, string longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

    }
}
