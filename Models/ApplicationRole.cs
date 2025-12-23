using Microsoft.AspNetCore.Identity;

namespace MyBlogApplication.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        // Inherited Constructors
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName)
            :base(roleName)
        {
            
        }


        // Extended Properties
        public string Description { get; set; } = null!;


        // Audit Columns
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
