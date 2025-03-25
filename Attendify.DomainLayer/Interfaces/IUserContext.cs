using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DomainLayer.Interfaces
{
    public interface IUserContext
    {
        bool IsInRole(Role role);
    }
    public enum Role { CanCreateEvents }
}
