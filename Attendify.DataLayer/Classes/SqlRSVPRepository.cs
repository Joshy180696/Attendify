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
    public class SqlRSVPRepository : IRSVPRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SqlRSVPRepository> _logger;

        public SqlRSVPRepository(AppDbContext context, ILogger<SqlRSVPRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void CreateRSVP(RSVP rsvp)
        {
            if (rsvp == null) throw new ArgumentNullException(nameof(rsvp), "RSVP Cannot be null!");

            _context.RSVPs.Add(rsvp);
        }
    }
}
