using FluentSearchEngine.Attributes;
using System.Text.Json.Serialization;

namespace FluentSearchEngine.Model
{
    public class GeoSearchModel<TKey> : SearchModel<TKey> where TKey : struct
    {
        [SearchFilter]
        [JsonPropertyName("_geo")]
        public GeoCoordinates GeoCoordinates { get; set; }

        public GeoCoordinates GetCoordinates()
        {
            return GeoCoordinates;
        }

        public void SetCoordinates(GeoCoordinates coordinates)
        {
            GeoCoordinates = coordinates;
        }
    }
}
