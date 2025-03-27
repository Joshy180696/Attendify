using Attendify.DomainLayer.DTO;
using Attendify.DomainLayer.Interfaces;
using Attendify.UILayer.Interface;
using Attendify.UILayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Attendify.UILayer.Controllers
{
    public class EventsController : Controller
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IEventService _eventService;
        private readonly IViewRenderService _viewRenderService;


        public EventsController(IEventService eventService, IViewRenderService viewRenderService, ILogger<EventsController> logger)
        {
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _viewRenderService = viewRenderService ?? throw new ArgumentNullException(nameof(viewRenderService)); ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }


        public async Task<IActionResult> Index(int? pageNumber, string sortBy = "", string sortDirection = "asc")
        {
            var events = await _eventService.GetAllPaginatedEventsAsync(pageNumber ?? 1, 10, string.Empty,
                null, null, null, true, sortBy, sortDirection);

            return View(events);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> LoadEvents(RequestEventsDto model)
        {
            var events = await _eventService.GetAllPaginatedEventsAsync(model.pageNumber, model.pageSize, model.searchString ?? "",
                model.year, model.month, model.day, model.showAll, model.sortBy, model.sortDirection);
            
            var html = await _viewRenderService.RenderViewToStringAsync("_EventsTable", events, ControllerContext); 
            return Json(new { html });
            
         
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Details(int id)
        {
            var eventEntity = await _eventService.GetEventDetailsAsync(id);
            if (eventEntity == null)
            {
                return NotFound();
            }
            return View(eventEntity);
        }



        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _eventService.DeleteEventAsync(id);
                return Json(new { response.success, response.message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting event with ID {EventId}", id);
                return Json(new { success = false, message = "Failed to delete event: " + ex.Message });
            }


        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
