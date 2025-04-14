using Newtonsoft.Json;

namespace Domain.Entities
{
    public class IdeaUpdates
    {
        public int IdeaId { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public virtual Idea Idea { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual Status Status { get; set; } = null!;

        [JsonProperty("@id")]
        public string RavenId { get; set; }
    }
}
