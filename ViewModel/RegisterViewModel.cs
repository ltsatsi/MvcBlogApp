using System.ComponentModel.DataAnnotations;

namespace MyBlogApplication.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(maximumLength: 20, MinimumLength = 3, 
            ErrorMessage = "First Name must be at least 3 to 20 characters")]
        public string FirstName { get; set; } = null!;



        [StringLength(maximumLength: 20, MinimumLength = 3,
            ErrorMessage = "Last Name must be at least 3 to 20 characters")]
        public string? LastName { get; set; }



        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email.")]
        public string Email { get; set; } = null!;



        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;



        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
