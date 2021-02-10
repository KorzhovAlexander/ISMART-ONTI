using System;
using System.Text;
using System.Threading.Tasks;
using AppExample.Core.Entities.Identity;
using AppExample.Core.Enums;
using AppExample.WebUI.Models;
using AppExample.WebUI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace AppExample.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailSenderService _emailSenderService;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            EmailSenderService emailSenderService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSenderService = emailSenderService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(
                loginModel.Email,
                loginModel.Password,
                loginModel.RememberMe,
                true);

            if (result.Succeeded) return Ok();

            if (result.IsLockedOut)
                return BadRequest($"Аккаунт с имененем {loginModel.Email} временно заблокирован");

            if (result.IsNotAllowed)
                return BadRequest($"Аккаунт не подтвержден");
            
            return BadRequest("Неверное имя пользователя или пароль");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            var user = new ApplicationUser {UserName = registerModel.Email, Email = registerModel.Email};

            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                
                var callbackUrl = $"https://localhost:5005/#/confirm-email?userId={user.Id}&code={code}";
                
                await _emailSenderService.SendEmailAsync(registerModel.Email, "Подтвердите аккаунт",
                    $"<div>Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>Подтверждение регистрации</a></div>");

                await _userManager.AddToRoleAsync(user, RolesEnum.User.ToString());
                await _signInManager.SignInAsync(user, false);

                return Ok(
                    "Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
            }


            return BadRequest(result.Errors);
        }
        

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Вы вышли из системы");
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId,string code)
        {
            
            if (userId == null || code == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Пользователь не найден");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result.Succeeded ?  (IActionResult) Ok() : BadRequest(result.Errors);
        }
    }
}