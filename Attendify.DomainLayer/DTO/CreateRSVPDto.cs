using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DomainLayer.DTO
{
    public class CreateRSVPDto
    {
        public int evId { get; set; } // Match frontend "eventId"
        public string userName { get; set; } = string.Empty; // Match "userName"
        public string response { get; set; } = string.Empty; // Match "response"
    }
}
