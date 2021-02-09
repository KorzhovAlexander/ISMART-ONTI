using System.ComponentModel.DataAnnotations;

namespace AppExample.WebUI.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "Длина пароля должна быть от 6 до 100 символов",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}