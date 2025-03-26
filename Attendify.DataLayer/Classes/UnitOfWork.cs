using Attendify.DataLayer.Context;
using Attendify.DomainLayer.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DataLayer.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        public IEventsRepository EventsRepository { get; }
        public IRSVPRepository RSVPRepository { get; }

        public UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger, IEventsRepository eventsRepository, IRSVPRepository rsvpRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            EventsRepository = eventsRepository ?? throw new ArgumentNullException(nameof(eventsRepository));
            RSVPRepository = rsvpRepository ?? throw new ArgumentNullException(nameof(rsvpRepository));
        }

        public async Task<int> CommitAsync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Transaction failed while committing changes to the database");
                throw;
            }

        }

        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }


    }
}
