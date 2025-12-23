namespace MyBlogApplication.ViewModel
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string RoleName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? CreatedOn { get; set; }
    }
}
