namespace Attendify.DomainLayer.DTO
{
    public class EventListDto
    {
        public int Id { get; set; } // Primary key
        public string Title { get; set; } = string.Empty; // e.g., "Game Night"
        public string Description { get; set; } = string.Empty; // e.g., "Bring your own snacks!"
        public DateTime DateTime { get; set; } // When it’s happening
        public string Location { get; set; } = string.Empty; // Where it’s at
        public string CreatedBy { get; set; } = string.Empty; // Simple string for now (could expand to a User model later)
        public int RSVPCount { get; set; }
        public List<RSVPDetailsDto> RSVPs { get; set; }
    }
}
