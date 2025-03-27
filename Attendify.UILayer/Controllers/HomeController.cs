using Attendify.DomainLayer.DTO;
using Attendify.DomainLayer.Interfaces;
using Attendify.UILayer.Interface;
using Attendify.UILayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Attendify.UILayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventService _eventService;
        private readonly IViewRenderService _viewRenderService;

        public HomeController(ILogger<HomeController> logger, IEventService eventService, IViewRenderService viewRenderService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _viewRenderService = viewRenderService ?? throw new ArgumentNullException(nameof(viewRenderService));
        }

        /// <summary>
        /// First render - Events for today
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var eventList = await _eventService.GetAllPaginatedEventsAsync(1, 5, string.Empty, null, null, null);

            return View(eventList);
        }

        /// <summary>
        /// Pagination - For today events
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> LoadEvents(RequestEventsDto model)
        {
            var events = await _eventService.GetAllPaginatedEventsAsync(
                model.pageNumber,
                model.pageSize,
                model.searchString ?? string.Empty,
                model.year,
                model.month,
                model.day
            );

            bool isEmpty = !events.Any();

            var renderHtml = await _viewRenderService.RenderViewToStringAsync("_EventsList", events, ControllerContext);

            return Json(new { html = renderHtml, pageNumber = model.pageNumber, isEmpty, searchString = model.searchString });
        }

        /// <summary>
        /// Create a new event
        /// </summary>
        /// <param name="eventDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([FromBody] CreateEventDto eventDto)
        {
            _logger.LogInformation("Create request received: {@EventDto}", eventDto);

            if (!ModelState.IsValid)
            {
                var errors = ModelState
               .Where(m => m.Value.Errors.Any())
               .ToDictionary(
                   kvp => kvp.Key, 
                   kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray() 
               );
                _logger.LogWarning("Validation failed: {@Errors}", errors);
                return BadRequest(new { success = false, errors });
            }
            await _eventService.AddEventAsync(eventDto);
            _logger.LogInformation("Event created successfully: {@Event}", eventDto);
            return Json(new { success = true });
        }

        /// <summary>
        /// Create a new rsvp for event
        /// </summary>
        /// <param name="rsvpDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RSVP([FromBody] CreateRSVPDto rsvpDto)
        {
            _logger.LogInformation("Received RSVP DTO: {@RSVP}", rsvpDto);

            try
            {
                var (rsvpId, success, message) = await _eventService.AddRSVPAsync(rsvpDto);
                return Json(new { rsvpId, success, message }); 
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error processing RSVP for event {EventId}", rsvpDto.evId);
                return BadRequest(new { success = false, error = "Failed to RSVP" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
