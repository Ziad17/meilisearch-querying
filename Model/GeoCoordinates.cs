using System.Text.Json.Serialization;

namespace FluentSearchEngine.Model
{
    public class GeoCoordinates
    {
        public GeoCoordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public GeoCoordinates()
        {
        }

        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonPropertyName("lng")]
        public double Longitude { get; set; }
    }
}
