using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DomainLayer.Models
{
    public class RSVP
    {
        public int Id { get; set; } 
        public int EventId { get; set; } 
        public Event Event { get; set; } 
        public string UserName { get; set; } = string.Empty; 
        public string Response { get; set; } = string.Empty; // "Yes", "No", "Maybe"
        public DateTime RSVPDate { get; set; } // When they RSVP’d (UTC)
    }
}
