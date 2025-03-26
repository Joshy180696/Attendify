using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DomainLayer.DTO
{
    public class RequestEventsDto
    {
        public int? year {  get; set; }
        public int? month { get; set; }
        public int? day { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string? searchString { get; set; }
        public bool showAll { get; set; }

        public string sortBy { get; set; } = string.Empty;
        public string sortDirection { get; set; } = string.Empty;


    }
}
