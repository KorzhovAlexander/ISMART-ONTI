using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using AppExample.Core.Entities.Identity;
using AppExample.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppExample.Application.Feature.User.Commands.DeleteUserRole
{
    public class DeleteUserRoleCommand : IRequest
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserRoleCommandHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(applicationUser => applicationUser.Id == request.UserId, cancellationToken);
            
            await _userManager.RemoveFromRoleAsync(user, ((RolesEnum) request.RoleId).ToString());
            
            return Unit.Value;
        }
    }
}