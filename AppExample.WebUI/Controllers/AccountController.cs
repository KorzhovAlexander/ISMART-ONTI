using System.Threading.Tasks;
using AppExample.Core.Entities.Identity;
using AppExample.Core.Enums;
using AppExample.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppExample.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(
                loginModel.UserName, 
                loginModel.Password, 
                loginModel.RememberMe,
                false);
            
            if (result.Succeeded) return Ok();
            
            
            return BadRequest("Неверное имя пользователя или пароль");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            var user = new ApplicationUser
            {
                UserName = registerModel.UserName,
                NormalizedUserName = registerModel.UserName.ToUpper(),
                EmailConfirmed = true
            };
            
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RolesEnum.Client.ToString());
                await _signInManager.SignInAsync(user, false);
                return Ok("Регистрация прошла успешно!");
            }
            

            return BadRequest(result.Errors);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Вы вышли из системы");
        }
    }
}