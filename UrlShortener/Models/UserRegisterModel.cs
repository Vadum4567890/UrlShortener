using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Поле 'Ім'я користувача' є обов'язковим.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Поле 'Пароль' є обов'язковим.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}