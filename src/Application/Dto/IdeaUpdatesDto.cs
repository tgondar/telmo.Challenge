namespace Application.Dto
{
    public class IdeaUpdatesDto
    {
        public string RavenId { get; set; }
        public int IdeaId { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public IdeaDto Idea { get; set; } = null!;
        public UserDto User { get; set; } = null!;
        public StatusDto Status { get; set; } = null!;
    }
}
