using Attendify.DomainLayer.DTO;
using Attendify.DomainLayer.Helpers;
using Attendify.DomainLayer.Interfaces;
using Attendify.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;



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

        public async Task<PaginatedList<EventListDto>> GetAllPaginatedEventsAsync(int pageNumber, int pageSize, string searchString, int? year = null, int? month = null, int? day = null,
            bool showAll = false, string sortBy = "", string sortDirection = "asc")
        {
            var eventsQuery = _unitOfWork.EventsRepository.GetEventList();

            // Apply date filter
            year ??= DateTime.UtcNow.Year;
            month ??= DateTime.UtcNow.Month;
            day ??= DateTime.UtcNow.Day;

            if (!showAll)
            {
                eventsQuery = eventsQuery.Where(e => e.DateTime.Year == year && e.DateTime.Month == month && e.DateTime.Day == day);
            }
           

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = $"%{searchString}%"; // Add wildcards for LIKE
                eventsQuery = eventsQuery.Where(e =>
                    EF.Functions.Like(e.Title, searchString) ||
                    EF.Functions.Like(e.Location, searchString) ||
                    (e.Description != null && EF.Functions.Like(e.Description, searchString)));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                bool isAscending = sortDirection?.ToLower() == "asc";
                eventsQuery = sortBy.ToLower() switch
                {
                    "date" => isAscending
                        ? eventsQuery.OrderBy(e => e.DateTime).ThenByDescending(e => e.RSVPs.Count())
                        : eventsQuery.OrderByDescending(e => e.DateTime).ThenByDescending(e => e.RSVPs.Count()),
                    "rsvpcount" => isAscending
                        ? eventsQuery.OrderBy(e => e.RSVPs.Count()).ThenByDescending(e => e.DateTime)
                        : eventsQuery.OrderByDescending(e => e.RSVPs.Count()).ThenByDescending(e => e.DateTime),
                    _ => eventsQuery.OrderByDescending(e => e.DateTime)
                };
            }
            else
            {
                eventsQuery = eventsQuery.OrderByDescending(e => e.DateTime);
            }

           

            // Project to DTO and paginate
            var eventDtosQuery = eventsQuery
                .Select(item => new EventListDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                DateTime = item.DateTime,
                CreatedBy = item.CreatedBy,
                Location = item.Location,
                RSVPCount = item.RSVPs.Count()
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
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new event: {@Event}", newEvent);
                throw;
            }
        }

        public async Task<(int rsvpId, bool success, string message)> AddRSVPAsync(CreateRSVPDto newRSVP)
        {
            if (newRSVP == null)
            {
                return (0, false, "RSVP data cannot be null");
            }
            if (string.IsNullOrEmpty(newRSVP.userName) || string.IsNullOrEmpty(newRSVP.response))
            {
                return (0, false, "Name and response are required");
            }
            if (!new[] { "Yes", "No", "Maybe" }.Contains(newRSVP.response))
            {
                return (0, false, "Response must be Yes, No, or Maybe");
            }

            var eventEntity = await _unitOfWork.EventsRepository.GetEventList()
                .FirstOrDefaultAsync(e => e.Id == newRSVP.evId);
            if (eventEntity == null)
            {
                return (0, false, "Event not found");
            }
            if (eventEntity.DateTime < DateTime.UtcNow)
            {
                return (0, false, "Cannot RSVP to past events");
            }

            try
            {
                var rsvpEntity = CreateNewRSVPEntity(newRSVP);
                _unitOfWork.RSVPRepository.CreateRSVP(rsvpEntity);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("RSVP created successfully for event {EventId}: {@RSVP}", newRSVP.evId, rsvpEntity);
                return (rsvpEntity.Id, true, "RSVP created successfully!");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error creating RSVP for event {EventId}: {@RSVP}", newRSVP.evId, newRSVP);
                throw; // Rethrow—let controller catch and format
            }
        }


        public async Task<EventListDto> GetEventDetailsAsync(int rsvpId)
        {
            var eventEntity = await _unitOfWork.EventsRepository.GetEventListWithDetails()
                 .FirstOrDefaultAsync(e => e.Id == rsvpId);


            if (eventEntity == null) 
            {
                return null;      
            }

            return new EventListDto
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title,
                DateTime = eventEntity.DateTime,
                RSVPs = eventEntity.RSVPs.Select(r => new RSVPDetailsDto
                {
                    UserName = r.UserName,
                    Response = r.Response,
                    RSVPDate = r.RSVPDate,
                }).ToList()
            };
        }

        public async Task<(bool success, string message)> DeleteEventAsync(int eventId)
        {
            if (eventId <= 0)
            {
                _logger.LogWarning("Delete event attempt failed: eventId {eventId} invalid ID", eventId);
                return (false, "Invalid Event ID");
            }

            var existingEvent = await _unitOfWork.EventsRepository.GetEventByIdAsync(eventId);

            if (existingEvent == null)
            {
                _logger.LogWarning("Delete event attempt failed: eventId {eventId} did not returned a valid record", eventId);
                return (false, $"Delete operation failed the Event ID: {eventId} did not returned any event");
            }

            try
            {
                _unitOfWork.EventsRepository.DeleteEvent(existingEvent);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("Delete event attempt succeeded: eventId {eventId}", eventId);
                return (true, $"The event and the rsvps were deleted successfully!");
            }
            catch (DbUpdateException ex)
            {

                _logger.LogError(ex, "Failed to delete event with ID {EventId} due to database error", eventId);
                return (false, "Failed to delete event due to a database error. Please try again later.");
            }

      
        }


        private RSVP CreateNewRSVPEntity(CreateRSVPDto newRSVP)
        {
            RSVP newRSVPEntity = new RSVP()
            {
                EventId = newRSVP.evId,
                UserName = newRSVP.userName,
                Response = newRSVP.response,
                RSVPDate = DateTime.UtcNow
            };
            return newRSVPEntity;
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
