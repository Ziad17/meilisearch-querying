using FluentSearchEngine.Attributes;
using System.Text.Json.Serialization;

namespace FluentSearchEngine.Model
{
    public class GeoSearchModel<TKey> : SearchModel<TKey> where TKey : struct
    {
        [Sortable]
        [SearchFilter]
        [JsonPropertyName("_geo")]
        public GeoCoordinates GeoCoordinates { get; set; }

        [JsonPropertyName("_geoDistance")]
        public int Distance { get; set; }
    }
}
