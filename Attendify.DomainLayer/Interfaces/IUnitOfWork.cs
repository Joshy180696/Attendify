using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DomainLayer.Interfaces
{
    public interface IUnitOfWork
    {
        IEventsRepository EventsRepository { get; }

        Task<int> CommitAsync();
    }
}
