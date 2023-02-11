using System.Text.Json.Serialization;
using FluentSearchEngine.Attributes;

namespace FluentSearchEngine.Model
{
    public abstract class GeoSearchModel<T> : SearchModel<T>
    {
        [Sortable]
        [SearchFilter]
        [JsonPropertyName("_geo")]
        public GeoCoordinates GeoCoordinates { get; set; }

        [JsonPropertyName("_geoDistance")]
        public int Distance { get; set; }
    }
}
