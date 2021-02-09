using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppExample.Application.Feature.User.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
    {
    }
    
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery,IEnumerable<UserDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllUsersQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users
                .Select(user=>new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName
                })
                .ToListAsync(cancellationToken);
            
            return users;
        }
    }
}