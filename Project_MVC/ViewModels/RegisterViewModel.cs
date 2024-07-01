using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(5,ErrorMessage ="Minimum Password Length is 5")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required")]
        [Compare("Password", ErrorMessage = "ConfirmPassword Password does not match Password ")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        public bool IsAgree { get; set; }

    }
}
