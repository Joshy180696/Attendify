using Attendify.DomainLayer.DTO;
using Attendify.DomainLayer.Helpers;
using Attendify.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DomainLayer.Interfaces
{
    public interface IEventService
    {
        Task<PaginatedList<EventListDto>> GetAllPaginatedEventsAsync(int pageNumber, int pageSize, string searchString, int? year = null, int? month = null, int? day = null);

        Task<int> AddEventAsync(CreateEventDto newEvent);

    }
}
