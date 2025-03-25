using Attendify.DomainLayer.DTO;
using Attendify.DomainLayer.Helpers;
using Attendify.DomainLayer.Interfaces;
using Attendify.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace Attendify.DomainLayer.Classes
{
    public class EventsService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EventsService> _logger;

        public EventsService(IUnitOfWork unitOfWork, ILogger<EventsService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PaginatedList<EventListDto>> GetAllPaginatedEventsAsync(int pageNumber, int pageSize, string searchString, int? year = null, int? month = null, int? day = null)
        {
            var eventsQuery = _unitOfWork.EventsRepository.GetEventList();

            // Apply date filter
            year ??= DateTime.UtcNow.Year;
            month ??= DateTime.UtcNow.Month;
            day ??= DateTime.UtcNow.Day;
            eventsQuery = eventsQuery.Where(e => e.DateTime.Year == year && e.DateTime.Month == month && e.DateTime.Day == day);

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                eventsQuery = eventsQuery.Where(e => e.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                                     e.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            // Project to DTO and paginate
            var eventDtosQuery = eventsQuery.Select(item => new EventListDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                DateTime = item.DateTime,
                CreatedBy = item.CreatedBy,
                Location = item.Location
            });

            return await PaginatedList<EventListDto>.CreateAsync(eventDtosQuery, pageNumber, pageSize);
        }

        public async Task<int> AddEventAsync(CreateEventDto newEvent)
        {
            if (newEvent == null)
            {
                throw new ArgumentNullException(nameof(newEvent), "CreateEvent model cannot be null");
            }
            try
            {
                var entity = CreateNewEventEntity(newEvent);
                _logger.LogInformation("Attempting to create event: {@Event}", entity); // Log before save
                _unitOfWork.EventsRepository.CreateEvent(entity);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("Event created successfully: {@Event}", entity);
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new event: {@Event}", newEvent);
                throw new ApplicationException("An error occurred while creating a new event", ex);
            }
        }

        private Event CreateNewEventEntity(CreateEventDto newEvent)
        {
            Event ev = new Event()
            {
                Id = 0,
                Title = newEvent.Title,
                Description = newEvent.Description,
                DateTime = newEvent.DateTime.ToUniversalTime(), 
                CreatedBy = newEvent.CreatedBy,
                Location = newEvent.Location,
            };
            return ev;
        }
    }
}
