using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class RPEnterEmailModel
    {
        [Required(ErrorMessage = "Введіть пошту")]
        [EmailAddress(ErrorMessage = "Невірний формат пошти")]
        public string Email { get; set; } = string.Empty;
    }
}
