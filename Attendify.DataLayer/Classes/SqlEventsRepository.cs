using Attendify.DataLayer.Context;
using Attendify.DomainLayer.Interfaces;
using Attendify.DomainLayer.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DataLayer.Classes
{
    public class SqlEventsRepository : IEventsRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SqlEventsRepository> _logger;

        public SqlEventsRepository(AppDbContext context, ILogger<SqlEventsRepository> logger)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IQueryable<Event> GetEventList()
        {
            try
            {
                return _context.Events.AsQueryable();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving events list");
                throw;
            }
        }

        public void CreateEvent(Event newEvent)
        {
            if (newEvent == null)
            {
                throw new ArgumentNullException(nameof(newEvent), "Event cannot be null!");
            }
             _context.Events.Add(newEvent);
        }
    }
}
