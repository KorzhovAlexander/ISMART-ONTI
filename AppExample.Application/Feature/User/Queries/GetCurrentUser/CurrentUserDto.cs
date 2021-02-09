using System.Collections.Generic;

namespace AppExample.Application.Feature.User.Queries.GetCurrentUser
{
    public class CurrentUserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }

        public bool IsAuthenticated { get; set; }

        public IEnumerable<RoleDto> Roles { get; set; }
    }
}