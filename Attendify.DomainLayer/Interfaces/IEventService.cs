using Attendify.DomainLayer.DTO;
using Attendify.DomainLayer.Helpers;
using Attendify.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DomainLayer.Interfaces
{
    public interface IEventService
    {
        Task<PaginatedList<EventListDto>> GetAllPaginatedEventsAsync(int pageNumber, int pageSize, string searchString, int? year = null, int? month = null, int? day = null, bool showAll = false, string sortBy = "", string sortDirection = "asc");

        Task<int> AddEventAsync(CreateEventDto newEvent);

        Task<(int rsvpId, bool success, string message)> AddRSVPAsync(CreateRSVPDto newRSVP);

        Task<EventListDto> GetEventDetailsAsync(int rsvpId);

        Task<(bool success, string message)> DeleteEventAsync(int eventId);

    }
}
