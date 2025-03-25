using Attendify.DomainLayer.Interfaces;

namespace Attendify.UILayer.Classes
{
    public class AspNetUserContextAdapter : IUserContext
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AspNetUserContextAdapter(HttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public bool IsInRole(Role role)
        {
            var user = _contextAccessor.HttpContext?.User;
            return user?.IsInRole(role.ToString()) ?? false;
        }
    }
}
