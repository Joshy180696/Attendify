using Attendify.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Attendify.DomainLayer.Interfaces
{
    public interface IEventsRepository
    {
        IQueryable<Event> GetEventList();
        IQueryable<Event> GetEventListWithDetails();
        void CreateEvent(Event newEvent);
        void DeleteEvent(Event ev);
        Task<Event> GetEventByIdAsync(int id);

    }
}
