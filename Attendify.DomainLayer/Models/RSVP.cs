using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DomainLayer.Models
{
    public class RSVP
    {
        public int Id { get; set; } // Primary key
        public int EventId { get; set; } // Foreign key to Event
        public Event Event { get; set; } = new Event(); // Navigation property
        public string UserName { get; set; } = string.Empty; // Simple string for now (could tie to a User model later)
        public string Response { get; set; } = string.Empty; // "Yes", "No", "Maybe"
    }
}
