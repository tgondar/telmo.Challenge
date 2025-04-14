using Newtonsoft.Json;
using System.Text.Json;

namespace Domain.Entities
{
    public class Idea
    {
        public int IdeaId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public DateTime PublishDate { get; set; }

        public DateTime UpdatedAt { get; set; }

        [JsonProperty("@id")]
        public string RavenId { get; set; }
    }
}
