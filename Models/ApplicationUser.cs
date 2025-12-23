using Microsoft.AspNetCore.Identity;

namespace MyBlogApplication.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        // Extended Properties
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? RoleName { get; set; }


        // Audit Columns
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }


        // Navigation Properties
        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
