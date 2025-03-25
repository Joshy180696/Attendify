using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DomainLayer.Models
{
    public class Event
    {
        public int Id { get; set; } // Primary key
        public string Title { get; set; } = string.Empty; // e.g., "Game Night"
        public string Description { get; set; } = string.Empty; // e.g., "Bring your own snacks!"
        public DateTime DateTime { get; set; } // When it’s happening
        public string Location { get; set; } = string.Empty; // Where it’s at
        public string CreatedBy { get; set; } = string.Empty; // Simple string for now (could expand to a User model later)
    }
}
