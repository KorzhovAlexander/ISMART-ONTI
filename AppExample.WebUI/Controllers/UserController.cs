using System.Threading.Tasks;
using AppExample.Application.Feature.User.Commands.AddUserRole;
using AppExample.Application.Feature.User.Commands.DeleteUserRole;
using AppExample.Application.Feature.User.Queries.GetAllUsers;
using AppExample.Application.Feature.User.Queries.GetCurrentUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppExample.WebUI.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        [AllowAnonymous]
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return Ok(await Mediator.Send(new GetCurrentUserQuery()));

            return Ok();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery()));
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddUserRoleAsync(AddUserRoleCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpDelete("delete-role")]
        public async Task<IActionResult> DeleteUserRoleAsync(DeleteUserRoleCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}