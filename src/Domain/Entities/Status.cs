using Newtonsoft.Json;

namespace Domain.Entities
{
    public class Status
    {
        public int StatusId { get; set; }
        public string Name { get; set; } = null!;

        [JsonProperty("@id")]
        public string RavenId { get; set; }
    }
}
