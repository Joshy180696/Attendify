using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DomainLayer.DTO
{
    public class RSVPDetailsDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public DateTime RSVPDate { get; set; }
    }
}
