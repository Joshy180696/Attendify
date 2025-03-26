using Attendify.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DomainLayer.Interfaces
{
    public interface IRSVPRepository
    {
        void CreateRSVP(RSVP rsvp);

    }
}
