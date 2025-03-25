using Attendify.DomainLayer.DTO;
using Attendify.DomainLayer.Interfaces;
using Attendify.UILayer.Interface;
using Attendify.UILayer.Models;
using Microsoft.AspNetCore.Mvc;
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
            var eventList = await _eventService.GetAllPaginatedEventsAsync(1, 1, string.Empty, null, null, null);

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
                model.searchString ?? "",
                model.year,
                model.month,
                model.day
            );

            bool isEmpty = !events.Any();

    
            var renderHtml = await _viewRenderService.RenderViewToStringAsync("_EventsList", events, ControllerContext);

            return Json(new { html = renderHtml, pageNumber = model.pageNumber, isEmpty });
        }

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
                   kvp => kvp.Key, // Field name (e.g., "Title")
                   kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray() // Array of error messages
               );
                _logger.LogWarning("Validation failed: {@Errors}", errors);
                return BadRequest(new { success = false, errors });
            }
            await _eventService.AddEventAsync(eventDto);
            _logger.LogInformation("Event created successfully: {@Event}", eventDto);
            return Json(new { success = true });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
