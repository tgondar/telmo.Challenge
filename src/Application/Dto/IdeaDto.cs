namespace Application.Dto
{
    public class IdeaDto
    {
        public string RavenId { get; set; }
        public int IdeaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<StatusDto> Status { get; set; }
    }
}
