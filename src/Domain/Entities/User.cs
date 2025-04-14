using Newtonsoft.Json;

namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public required string UserName { get; set; }

        [JsonProperty("@id")]
        public string RavenId { get; set; }
    }
}
